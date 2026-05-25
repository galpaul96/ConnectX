using ConnectX.Domain.MongoDb.Students;

namespace ConnectX.Api.MongoDb.Students;

public interface IStudentRepository
{
    Task<Student> CreateAsync(Student student, CancellationToken cancellationToken = default);
    Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Student student, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
