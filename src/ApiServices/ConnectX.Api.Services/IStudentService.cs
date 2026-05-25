using ConnectX.Domain.Services.Students;

namespace ConnectX.Api.Services;

public interface IStudentService
{
    Task<StudentModel> CreateAsync(
        CreateStudentModel student,
        CancellationToken cancellationToken = default);

    Task<StudentModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<StudentModel>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        UpdateStudentModel student,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
