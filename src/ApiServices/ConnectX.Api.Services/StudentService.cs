using AutoMapper;
using ConnectX.Api.MongoDb.Students;
using ConnectX.Domain.MongoDb.Students;
using ConnectX.Domain.Services.Students;

namespace ConnectX.Api.Services;

internal class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly IMapper _mapper;

    public StudentService(
        IStudentRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StudentModel> CreateAsync(
        CreateStudentModel student,
        CancellationToken cancellationToken = default)
    {
        var createdStudent = await _repository.CreateAsync(
            _mapper.Map<Student>(student),
            cancellationToken);

        return _mapper.Map<StudentModel>(createdStudent);
    }

    public async Task<StudentModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var student = await _repository.GetByIdAsync(id, cancellationToken);

        return student is null ? null : _mapper.Map<StudentModel>(student);
    }

    public async Task<IReadOnlyCollection<StudentModel>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var students = await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IReadOnlyCollection<StudentModel>>(students);
    }

    public async Task<bool> UpdateAsync(
        UpdateStudentModel student,
        CancellationToken cancellationToken = default)
    {
        var existingStudent = await _repository.GetByIdAsync(student.Id, cancellationToken);

        if (existingStudent is null)
        {
            return false;
        }

        var studentToUpdate = _mapper.Map(student, existingStudent);

        return await _repository.UpdateAsync(studentToUpdate, cancellationToken);
    }

    public Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(id, cancellationToken);
    }

    public Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _repository.ExistsAsync(id, cancellationToken);
    }
}
