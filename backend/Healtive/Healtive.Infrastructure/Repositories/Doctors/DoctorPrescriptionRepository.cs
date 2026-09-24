using System.Data;
using Dapper;
using Healtive.Application.DTOs.Doctor.Prescription;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorPrescriptionRepository : IDoctorPrescriptionRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorPrescriptionRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    // =========================================================
    // GET PRESCRIPTIONS BY APPOINTMENT
    // =========================================================

    public async Task<IEnumerable<PrescriptionResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    pr.Id,
    pr.PrescriptionNumber,
    pr.HospitalId,
    pr.BranchId,
    pr.AppointmentId,
    pr.PatientId,
    pr.DoctorId,
    pr.PrescriptionDate,
    pr.Diagnosis,
    pr.ClinicalNotes,
    pr.Advice,
    pr.FollowUpDate,
    pr.IsFinalized,
    pr.CreatedAt,
    pr.UpdatedAt

FROM Prescriptions pr

WHERE pr.AppointmentId = @AppointmentId
AND pr.HospitalId = @HospitalId
AND pr.DoctorId = @DoctorId

ORDER BY pr.PrescriptionDate DESC, pr.CreatedAt DESC;";

        var prescriptions = (
            await connection.QueryAsync<PrescriptionResponse>(
                sql,
                new
                {
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    AppointmentId = appointmentId
                })).ToList();

        if (prescriptions.Count == 0)
            return prescriptions;

        await LoadItemsAsync(
            connection,
            null,
            prescriptions);

        return prescriptions;
    }

    // =========================================================
    // GET PRESCRIPTION BY ID
    // =========================================================

    public async Task<PrescriptionResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    pr.Id,
    pr.PrescriptionNumber,
    pr.HospitalId,
    pr.BranchId,
    pr.AppointmentId,
    pr.PatientId,
    pr.DoctorId,
    pr.PrescriptionDate,
    pr.Diagnosis,
    pr.ClinicalNotes,
    pr.Advice,
    pr.FollowUpDate,
    pr.IsFinalized,
    pr.CreatedAt,
    pr.UpdatedAt

FROM Prescriptions pr

WHERE pr.Id = @PrescriptionId
AND pr.HospitalId = @HospitalId
AND pr.DoctorId = @DoctorId

LIMIT 1;";

        var prescription =
            await connection.QueryFirstOrDefaultAsync<PrescriptionResponse>(
                sql,
                new
                {
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    PrescriptionId = prescriptionId
                });

        if (prescription == null)
            return null;

        await LoadItemsAsync(
            connection,
            null,
            new List<PrescriptionResponse>
            {
                prescription
            });

        return prescription;
    }

    // =========================================================
    // CREATE
    // =========================================================

    public async Task<PrescriptionResponse> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreatePrescriptionRequest request)
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
            // 2. VALIDATE DOSAGES
            // =====================================================

            await ValidateDosagesAsync(
                connection,
                transaction,
                request.Items);

            // =====================================================
            // 3. INSERT PRESCRIPTION
            // =====================================================

            var prescriptionId = Guid.NewGuid();

            const string insertSql = @"
INSERT INTO Prescriptions
(
    Id,
    PrescriptionNumber,
    HospitalId,
    BranchId,
    AppointmentId,
    PatientId,
    DoctorId,
    PrescriptionDate,
    Diagnosis,
    ClinicalNotes,
    Advice,
    FollowUpDate,
    IsFinalized,
    CreatedAt
)
VALUES
(
    @Id,
    @PrescriptionNumber,
    @HospitalId,
    @BranchId,
    @AppointmentId,
    @PatientId,
    @DoctorId,
    CURRENT_TIMESTAMP,
    @Diagnosis,
    @ClinicalNotes,
    @Advice,
    @FollowUpDate,
    FALSE,
    CURRENT_TIMESTAMP
);";

            await connection.ExecuteAsync(
                insertSql,
                new
                {
                    Id = prescriptionId,
                    PrescriptionNumber = GeneratePrescriptionNumber(),
                    HospitalId = hospitalId,
                    BranchId = appointment.BranchId,
                    AppointmentId = request.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId = doctorId,
                    request.Diagnosis,
                    request.ClinicalNotes,
                    request.Advice,
                    request.FollowUpDate
                },
                transaction);

            // =====================================================
            // 4. INSERT PRESCRIPTION ITEMS
            // =====================================================

            await InsertItemsAsync(
                connection,
                transaction,
                prescriptionId,
                request.Items);

            // =====================================================
            // 5. COMMIT
            // =====================================================

            transaction.Commit();

            var prescription = await GetByIdAsync(
                hospitalId,
                doctorId,
                prescriptionId);

            return prescription!;
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    public async Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId,
        UpdatePrescriptionRequest request)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // =====================================================
            // 1. UPDATE PRESCRIPTION HEADER
            // =====================================================

            const string updateSql = @"
UPDATE Prescriptions

SET
    Diagnosis = @Diagnosis,
    ClinicalNotes = @ClinicalNotes,
    Advice = @Advice,
    FollowUpDate = @FollowUpDate,
    IsFinalized = @IsFinalized,
    UpdatedAt = CURRENT_TIMESTAMP

WHERE Id = @PrescriptionId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

            var rowsAffected = await connection.ExecuteAsync(
                updateSql,
                new
                {
                    PrescriptionId = prescriptionId,
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    request.Diagnosis,
                    request.ClinicalNotes,
                    request.Advice,
                    request.FollowUpDate,
                    request.IsFinalized
                },
                transaction);

            if (rowsAffected == 0)
            {
                transaction.Rollback();

                return false;
            }

            // =====================================================
            // 2. VALIDATE DOSAGES
            // =====================================================

            await ValidateDosagesAsync(
                connection,
                transaction,
                request.Items);

            // =====================================================
            // 3. SYNC PRESCRIPTION ITEMS
            // =====================================================

            const string deleteItemsSql = @"
DELETE FROM PrescriptionItems

WHERE PrescriptionId = @PrescriptionId;";

            await connection.ExecuteAsync(
                deleteItemsSql,
                new
                {
                    PrescriptionId = prescriptionId
                },
                transaction);

            await InsertItemsAsync(
                connection,
                transaction,
                prescriptionId,
                request.Items);

            // =====================================================
            // 4. COMMIT
            // =====================================================

            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }

    // =========================================================
    // DELETE
    // =========================================================

    public async Task<bool> DeleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // =====================================================
            // 1. VERIFY OWNERSHIP
            // =====================================================

            const string existsSql = @"
SELECT COUNT(*)

FROM Prescriptions

WHERE Id = @PrescriptionId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

            var exists = await connection.ExecuteScalarAsync<int>(
                existsSql,
                new
                {
                    PrescriptionId = prescriptionId,
                    HospitalId = hospitalId,
                    DoctorId = doctorId
                },
                transaction);

            if (exists == 0)
            {
                transaction.Rollback();

                return false;
            }

            // =====================================================
            // 2. DELETE ITEMS FIRST (FK CONSTRAINT)
            // =====================================================

            const string deleteItemsSql = @"
DELETE FROM PrescriptionItems

WHERE PrescriptionId = @PrescriptionId;";

            await connection.ExecuteAsync(
                deleteItemsSql,
                new
                {
                    PrescriptionId = prescriptionId
                },
                transaction);

            // =====================================================
            // 3. DELETE PRESCRIPTION
            // =====================================================

            const string deleteSql = @"
DELETE FROM Prescriptions

WHERE Id = @PrescriptionId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

            await connection.ExecuteAsync(
                deleteSql,
                new
                {
                    PrescriptionId = prescriptionId,
                    HospitalId = hospitalId,
                    DoctorId = doctorId
                },
                transaction);

            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }

    // =========================================================
    // FINALIZE
    // =========================================================

    public async Task<bool> FinalizeAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Prescriptions

SET
    IsFinalized = TRUE,
    UpdatedAt = CURRENT_TIMESTAMP

WHERE Id = @PrescriptionId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                PrescriptionId = prescriptionId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });

        return rowsAffected > 0;
    }

    // =========================================================
    // HELPERS
    // =========================================================

    private async Task ValidateDosagesAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        IEnumerable<PrescriptionItemRequest> items)
    {
        const string dosageSql = @"
SELECT COUNT(*)

FROM MedicineDosages

WHERE Id = @DosageId
AND IsActive = 1;";

        foreach (var item in items)
        {
            var exists = await connection.ExecuteScalarAsync<int>(
                dosageSql,
                new
                {
                    DosageId = item.DosageId
                },
                transaction);

            if (exists == 0)
            {
                throw new InvalidOperationException(
                    "Invalid or inactive dosage for a prescription item.");
            }
        }
    }

    private static async Task InsertItemsAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        Guid prescriptionId,
        IEnumerable<PrescriptionItemRequest> items)
    {
        const string insertSql = @"
INSERT INTO PrescriptionItems
(
    Id,
    PrescriptionId,
    MedicineName,
    DosageId,
    Strength,
    Route,
    Frequency,
    DurationDays,
    Quantity,
    Instructions,
    CreatedAt
)
VALUES
(
    @Id,
    @PrescriptionId,
    @MedicineName,
    @DosageId,
    @Strength,
    @Route,
    @Frequency,
    @DurationDays,
    @Quantity,
    @Instructions,
    CURRENT_TIMESTAMP
);";

        foreach (var item in items)
        {
            await connection.ExecuteAsync(
                insertSql,
                new
                {
                    Id = Guid.NewGuid(),
                    PrescriptionId = prescriptionId,
                    item.MedicineName,
                    item.DosageId,
                    item.Strength,
                    item.Route,
                    item.Frequency,
                    item.DurationDays,
                    item.Quantity,
                    item.Instructions
                },
                transaction);
        }
    }

    private static async Task LoadItemsAsync(
        IDbConnection connection,
        IDbTransaction? transaction,
        IReadOnlyCollection<PrescriptionResponse> prescriptions)
    {
        if (prescriptions.Count == 0)
            return;

        var prescriptionIds = prescriptions
            .Select(x => x.Id)
            .ToList();

        const string sql = @"
SELECT
    pi.Id,
    pi.PrescriptionId,
    pi.MedicineName,
    pi.DosageId,
    md.Name AS DosageName,
    pi.Strength,
    pi.Route,
    pi.Frequency,
    pi.DurationDays,
    pi.Quantity,
    pi.Instructions,
    pi.CreatedAt

FROM PrescriptionItems pi

INNER JOIN MedicineDosages md
    ON md.Id = pi.DosageId

WHERE pi.PrescriptionId IN @PrescriptionIds

ORDER BY pi.CreatedAt ASC;";

        var items = (
            await connection.QueryAsync<PrescriptionItemResponse>(
                sql,
                new
                {
                    PrescriptionIds = prescriptionIds
                },
                transaction)).ToList();

        var itemsByPrescription = items
            .GroupBy(x => x.PrescriptionId)
            .ToDictionary(
                g => g.Key,
                g => g.ToList());

        foreach (var prescription in prescriptions)
        {
            if (itemsByPrescription.TryGetValue(
                prescription.Id,
                out var prescriptionItems))
            {
                prescription.Items = prescriptionItems;
            }
        }
    }

    private static string GeneratePrescriptionNumber()
    {
        return $"RX-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }

    private class AppointmentOwnershipDbModel
    {
        public Guid PatientId { get; set; }

        public Guid BranchId { get; set; }
    }
}