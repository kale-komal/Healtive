using System.Data;
using Dapper;
using Healtive.Application.DTOs.Doctor.Lab;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorLabRepository : IDoctorLabRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorLabRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    // =========================================================
    // GET ACTIVE LAB CATEGORIES
    // =========================================================

    public async Task<IEnumerable<LabCategoryResponse>> GetActiveCategoriesAsync()
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    lc.Id,
    lc.Name,
    lc.Code,
    lc.Description,
    lc.DisplayOrder

FROM LabCategories lc

WHERE lc.IsActive = 1

ORDER BY lc.DisplayOrder ASC, lc.Name ASC;";

        return await connection.QueryAsync<LabCategoryResponse>(sql);
    }

    // =========================================================
    // GET ACTIVE LAB TESTS
    // =========================================================

    public async Task<IEnumerable<LabTestResponse>> GetActiveTestsAsync(
        Guid? categoryId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    lt.Id,
    lt.TestCode,
    lt.CategoryId,
    lc.Name AS CategoryName,
    lt.Name,
    lt.Description,
    lt.Price,
    lt.NormalRange,
    lt.Unit

FROM LabTests lt

INNER JOIN LabCategories lc
    ON lc.Id = lt.CategoryId

WHERE lt.IsActive = 1
AND (@CategoryId IS NULL OR lt.CategoryId = @CategoryId)

ORDER BY lc.DisplayOrder ASC, lt.Name ASC;";

        return await connection.QueryAsync<LabTestResponse>(
            sql,
            new
            {
                CategoryId = categoryId
            });
    }

    // =========================================================
    // GET LAB ORDERS BY APPOINTMENT
    // =========================================================

    public async Task<IEnumerable<LabOrderResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    plo.Id,
    plo.OrderNumber,
    plo.PatientId,
    plo.AppointmentId,
    plo.DoctorId,
    plo.LabTestId,
    lt.Name AS LabTestName,
    lt.TestCode,
    plo.OrderDate,
    plo.Status,
    plo.Remarks,
    plo.CreatedAt

FROM PatientLabOrders plo

INNER JOIN LabTests lt
    ON lt.Id = plo.LabTestId

INNER JOIN Doctors d
    ON d.Id = plo.DoctorId
    AND d.HospitalId = @HospitalId

WHERE plo.AppointmentId = @AppointmentId
AND plo.DoctorId = @DoctorId

ORDER BY plo.OrderDate DESC, plo.CreatedAt DESC;";

        return await connection.QueryAsync<LabOrderResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                AppointmentId = appointmentId
            });
    }

    // =========================================================
    // GET LAB ORDER BY ID
    // =========================================================

    public async Task<LabOrderResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid orderId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    plo.Id,
    plo.OrderNumber,
    plo.PatientId,
    plo.AppointmentId,
    plo.DoctorId,
    plo.LabTestId,
    lt.Name AS LabTestName,
    lt.TestCode,
    plo.OrderDate,
    plo.Status,
    plo.Remarks,
    plo.CreatedAt

FROM PatientLabOrders plo

INNER JOIN LabTests lt
    ON lt.Id = plo.LabTestId

INNER JOIN Doctors d
    ON d.Id = plo.DoctorId
    AND d.HospitalId = @HospitalId

WHERE plo.Id = @OrderId
AND plo.DoctorId = @DoctorId

LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<LabOrderResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                OrderId = orderId
            });
    }

    // =========================================================
    // CREATE LAB ORDERS
    // =========================================================

    public async Task<IEnumerable<LabOrderResponse>> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateLabOrderRequest request)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // =====================================================
            // 1. VERIFY APPOINTMENT OWNERSHIP
            // =====================================================

            const string appointmentSql = @"
SELECT
    PatientId,
    BranchId

FROM Appointments

WHERE Id = @AppointmentId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId

LIMIT 1;";

            var appointment =
                await connection.QueryFirstOrDefaultAsync<AppointmentOwnershipDbModel>(
                    appointmentSql,
                    new
                    {
                        AppointmentId = request.AppointmentId,
                        HospitalId = hospitalId,
                        DoctorId = doctorId
                    },
                    transaction);

            if (appointment == null)
            {
                throw new InvalidOperationException(
                    "Appointment not found or does not belong to the logged-in doctor.");
            }

            // =====================================================
            // 2. VALIDATE LAB TESTS
            // =====================================================

            await ValidateTestsAsync(
                connection,
                transaction,
                request.TestIds);

            // =====================================================
            // 3. INSERT ONE LAB ORDER ROW PER SELECTED TEST
            // =====================================================

            var testIds = request.TestIds.Distinct().ToList();

            var orderIds = new List<Guid>();

            const string insertSql = @"
INSERT INTO PatientLabOrders
(
    Id,
    OrderNumber,
    PatientId,
    AppointmentId,
    DoctorId,
    LabTestId,
    OrderDate,
    Status,
    Remarks,
    CreatedAt
)
VALUES
(
    @Id,
    @OrderNumber,
    @PatientId,
    @AppointmentId,
    @DoctorId,
    @LabTestId,
    CURRENT_TIMESTAMP,
    'Pending',
    @Remarks,
    CURRENT_TIMESTAMP
);";

            var baseOrderNumber = GenerateLabOrderNumber();

            for (var i = 0; i < testIds.Count; i++)
            {
                var orderId = Guid.NewGuid();

                await connection.ExecuteAsync(
                    insertSql,
                    new
                    {
                        Id = orderId,
                        OrderNumber = i == 0
                            ? baseOrderNumber
                            : $"{baseOrderNumber}-{i + 1}",
                        PatientId = appointment.PatientId,
                        AppointmentId = request.AppointmentId,
                        DoctorId = doctorId,
                        LabTestId = testIds[i],
                        request.Remarks
                    },
                    transaction);

                orderIds.Add(orderId);
            }

            // =====================================================
            // 4. COMMIT
            // =====================================================

            transaction.Commit();

            var orders = await LoadLabOrdersByIdsAsync(
                connection,
                hospitalId,
                doctorId,
                orderIds);

            return orders;
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }

    // =========================================================
    // UPDATE LAB ORDER
    // =========================================================

    public async Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid orderId,
        UpdateLabOrderRequest request)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE PatientLabOrders

SET
    Remarks = @Remarks,
    Status = @Status

WHERE Id = @OrderId
AND DoctorId = @DoctorId
AND Status NOT IN ('Completed', 'Cancelled')
AND EXISTS
(
    SELECT 1
    FROM Doctors d
    WHERE d.Id = PatientLabOrders.DoctorId
    AND d.HospitalId = @HospitalId
);";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                OrderId = orderId,
                DoctorId = doctorId,
                HospitalId = hospitalId,
                request.Remarks,
                request.Status
            });

        return rowsAffected > 0;
    }

    // =========================================================
    // CANCEL LAB ORDER
    // =========================================================

    public async Task<bool> CancelAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid orderId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE PatientLabOrders

SET
    Status = 'Cancelled'

WHERE Id = @OrderId
AND DoctorId = @DoctorId
AND Status <> 'Completed'
AND EXISTS
(
    SELECT 1
    FROM Doctors d
    WHERE d.Id = PatientLabOrders.DoctorId
    AND d.HospitalId = @HospitalId
);";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                OrderId = orderId,
                DoctorId = doctorId,
                HospitalId = hospitalId
            });

        return rowsAffected > 0;
    }

    // =========================================================
    // HELPERS
    // =========================================================

    private async Task ValidateTestsAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        IReadOnlyCollection<Guid> testIds)
    {
        const string testSql = @"
SELECT COUNT(*)

FROM LabTests

WHERE Id = @TestId
AND IsActive = 1;";

        foreach (var testId in testIds)
        {
            var exists = await connection.ExecuteScalarAsync<int>(
                testSql,
                new
                {
                    TestId = testId
                },
                transaction);

            if (exists == 0)
            {
                throw new InvalidOperationException(
                    "Invalid or inactive lab test selected.");
            }
        }
    }

    private async Task<List<LabOrderResponse>> LoadLabOrdersByIdsAsync(
        IDbConnection connection,
        Guid hospitalId,
        Guid doctorId,
        IEnumerable<Guid> orderIds)
    {
        const string sql = @"
SELECT
    plo.Id,
    plo.OrderNumber,
    plo.PatientId,
    plo.AppointmentId,
    plo.DoctorId,
    plo.LabTestId,
    lt.Name AS LabTestName,
    lt.TestCode,
    plo.OrderDate,
    plo.Status,
    plo.Remarks,
    plo.CreatedAt

FROM PatientLabOrders plo

INNER JOIN LabTests lt
    ON lt.Id = plo.LabTestId

INNER JOIN Doctors d
    ON d.Id = plo.DoctorId
    AND d.HospitalId = @HospitalId

WHERE plo.Id IN @OrderIds
AND plo.DoctorId = @DoctorId

ORDER BY plo.CreatedAt ASC;";

        return (await connection.QueryAsync<LabOrderResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                OrderIds = orderIds
            })).ToList();
    }

    private static string GenerateLabOrderNumber()
    {
        return $"LAB-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }

    private class AppointmentOwnershipDbModel
    {
        public Guid PatientId { get; set; }

        public Guid BranchId { get; set; }
    }
}