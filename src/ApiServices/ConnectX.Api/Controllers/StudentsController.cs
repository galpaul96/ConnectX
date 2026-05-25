using AutoMapper;
using ConnectX.Api.Services;
using ConnectX.Domain.Api.Students;
using ConnectX.Domain.Services.Students;
using Microsoft.AspNetCore.Mvc;

namespace ConnectX.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IStudentService studentService,
        IMapper mapper,
        ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet(Name = "GetStudents")]
    [ProducesResponseType(typeof(IReadOnlyCollection<StudentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<StudentResponse>>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting students.");

        var students = await _studentService.GetAllAsync(cancellationToken);

        return Ok(_mapper.Map<IReadOnlyCollection<StudentResponse>>(students));
    }

    [HttpGet("{id:guid}", Name = "GetStudent")]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting student {StudentId}.", id);

        var student = await _studentService.GetByIdAsync(id, cancellationToken);

        if (student is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<StudentResponse>(student));
    }

    [HttpGet("{id:guid}/exists", Name = "StudentExists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking if student {StudentId} exists.", id);

        var exists = await _studentService.ExistsAsync(id, cancellationToken);

        return Ok(exists);
    }

    [HttpPost(Name = "CreateStudent")]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StudentResponse>> CreateAsync(
        CreateStudentRequest student,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating student {StudentNumber}.", student.StudentNumber);

        var createdStudent = await _studentService.CreateAsync(
            _mapper.Map<CreateStudentModel>(student),
            cancellationToken);
        var response = _mapper.Map<StudentResponse>(createdStudent);

        return CreatedAtRoute(
            "GetStudent",
            new { id = response.Id },
            response);
    }

    [HttpPut("{id:guid}", Name = "UpdateStudent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        UpdateStudentRequest student,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating student {StudentId}.", id);

        var updateStudentModel = _mapper.Map<UpdateStudentModel>(student);
        updateStudentModel.Id = id;

        var updated = await _studentService.UpdateAsync(updateStudentModel, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteStudent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting student {StudentId}.", id);

        var deleted = await _studentService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
