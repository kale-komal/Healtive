namespace Healtive.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }

    Guid HospitalId { get; }

    Guid? BranchId { get; }

    string Role { get; }
}