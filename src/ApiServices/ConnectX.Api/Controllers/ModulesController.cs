using AutoMapper;
using ConnectX.Api.Services;
using ConnectX.Domain.Api.Modules;
using Microsoft.AspNetCore.Mvc;

namespace ConnectX.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ModulesController : ControllerBase
{
    private readonly IMeetingService _meetingService;
    private readonly IMapper _mapper;
    private readonly ILogger<ModulesController> _logger;

    public ModulesController(
        IMeetingService meetingService,
        IMapper mapper,
        ILogger<ModulesController> logger)
    {
        _meetingService = meetingService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("students/{studentId:guid}/upcoming-events", Name = "GetStudentUpcomingModuleEvents")]
    [ProducesResponseType(typeof(IReadOnlyList<StudentUpcomingEventResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StudentUpcomingEventResponse>>> GetUpcomingEventsAsync(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting upcoming module events for student {StudentId}.", studentId);

        var events = await _meetingService.GetAsync(studentId, cancellationToken);

        return Ok(_mapper.Map<IReadOnlyList<StudentUpcomingEventResponse>>(events));
    }

    [HttpGet("students/{studentId:guid}/upcoming-events/range", Name = "GetStudentUpcomingModuleEventsByRange")]
    [ProducesResponseType(typeof(IReadOnlyList<StudentUpcomingEventResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<StudentUpcomingEventResponse>>> GetUpcomingEventsAsync(
        Guid studentId,
        [FromQuery] DateTimeOffset startsAt,
        [FromQuery] DateTimeOffset endsAt,
        CancellationToken cancellationToken)
    {
        if (endsAt < startsAt)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid date range.",
                Detail = "The end date must be greater than or equal to the start date.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        _logger.LogInformation(
            "Getting upcoming module events for student {StudentId} between {StartsAt} and {EndsAt}.",
            studentId,
            startsAt,
            endsAt);

        var events = await _meetingService.GetAsync(studentId, startsAt, endsAt, cancellationToken);

        return Ok(_mapper.Map<IReadOnlyList<StudentUpcomingEventResponse>>(events));
    }

    [HttpGet("students/{studentId:guid}/meetings/{meetingId:guid}", Name = "GetStudentModuleMeeting")]
    [ProducesResponseType(typeof(StudentMeetingDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentMeetingDetailResponse>> GetMeetingAsync(
        Guid studentId,
        Guid meetingId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting meeting {MeetingId} details for student {StudentId}.",
            meetingId,
            studentId);

        var meeting = await _meetingService.GetAsync(studentId, meetingId, cancellationToken);

        if (meeting is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<StudentMeetingDetailResponse>(meeting));
    }
}

