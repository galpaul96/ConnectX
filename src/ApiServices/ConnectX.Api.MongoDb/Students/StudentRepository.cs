using ConnectX.Domain.MongoDb.Students;
using MongoDB.Driver;

namespace ConnectX.Api.MongoDb.Students;

public class StudentRepository : IStudentRepository
{
    private const string CollectionName = "students";

    private readonly IMongoCollection<Student> _collection;

    public StudentRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Student>(CollectionName);
    }

    public async Task<Student> CreateAsync(
        Student student,
        CancellationToken cancellationToken = default)
    {
        if (student.Id == Guid.Empty)
        {
            student.Id = Guid.NewGuid();
        }

        student.CreatedDate = DateTime.UtcNow;

        await _collection.InsertOneAsync(student, cancellationToken: cancellationToken);

        return student;
    }

    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(student => student.Id == id && !student.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(student => !student.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        Student student,
        CancellationToken cancellationToken = default)
    {
        student.UpdatedDate = DateTime.UtcNow;

        var result = await _collection.ReplaceOneAsync(
            existingStudent => existingStudent.Id == student.Id && !existingStudent.IsDeleted,
            student,
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var update = Builders<Student>.Update
            .Set(student => student.IsDeleted, true)
            .Set(student => student.DeletedDate, now)
            .Set(student => student.UpdatedDate, now);

        var result = await _collection.UpdateOneAsync(
            student => student.Id == id && !student.IsDeleted,
            update,
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(student => student.Id == id && !student.IsDeleted)
            .AnyAsync(cancellationToken);
    }
}
