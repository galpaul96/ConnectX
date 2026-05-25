using AutoMapper;
using ConnectX.Domain.Api.Modules;
using ConnectX.Domain.Api.Students;
using ConnectX.Domain.Entities;
using ConnectX.Domain.MongoDb.Students;
using ConnectX.Domain.Services.Students;

namespace ConnectX.Api.Services
{
    internal class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile()
        {
            CreateMap<CreateStudentRequest, CreateStudentModel>();
            CreateMap<UpdateStudentRequest, UpdateStudentModel>();
            CreateMap<StudentModel, StudentResponse>();

            CreateMap<CreateStudentModel, Student>();
            CreateMap<UpdateStudentModel, Student>();
            CreateMap<Student, StudentModel>();

            CreateMap<StudentUpcomingEventType, StudentUpcomingEventKind>()
                .ConvertUsing(x => (StudentUpcomingEventKind)x);
            CreateMap<StudentUpcomingEventModel, StudentUpcomingEventResponse>();
            CreateMap<StudentMeetingDetailModel, StudentMeetingDetailResponse>();
            CreateMap<LearningObjectiveModel, LearningObjectiveResponse>();
            CreateMap<LearningActivityModel, LearningActivityResponse>();
            CreateMap<AssignmentSummaryModel, AssignmentSummaryResponse>();
            CreateMap<ResourceSummaryModel, ResourceSummaryResponse>();
            CreateMap<LessonContentSummaryModel, LessonContentSummaryResponse>();
            CreateMap<StudyTipSummaryModel, StudyTipSummaryResponse>();
            CreateMap<LocationDirectionModel, LocationDirectionResponse>();

            CreateMap<Meeting, StudentUpcomingEventModel>()
                .ForMember(x => x.EventType, x => x.MapFrom(_ => StudentUpcomingEventType.Meeting))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Description))
                .ForMember(x => x.ModuleId, x => x.MapFrom(y => y.ModuleOffering != null && y.ModuleOffering.ProgramModule != null ? y.ModuleOffering.ProgramModule.Id : (Guid?)null))
                .ForMember(x => x.ModuleCode, x => x.MapFrom(y => y.ModuleOffering != null && y.ModuleOffering.ProgramModule != null ? y.ModuleOffering.ProgramModule.Code : null))
                .ForMember(x => x.ModuleName, x => x.MapFrom(y => y.ModuleOffering != null && y.ModuleOffering.ProgramModule != null ? y.ModuleOffering.ProgramModule.Name : null))
                .ForMember(x => x.TeacherId, x => x.MapFrom(y => y.ModuleOffering != null && y.ModuleOffering.Teacher != null ? y.ModuleOffering.Teacher.Id : (Guid?)null))
                .ForMember(x => x.TeacherName, x => x.MapFrom(y => y.ModuleOffering != null && y.ModuleOffering.Teacher != null ? y.ModuleOffering.Teacher.DisplayName : null))
                .ForMember(x => x.TeacherEmail, x => x.MapFrom(y => y.ModuleOffering != null && y.ModuleOffering.Teacher != null ? y.ModuleOffering.Teacher.Email : null))
                .ForMember(x => x.LocationName, x => x.MapFrom(y => y.AcademicLocation != null ? y.AcademicLocation.Name : y.Location))
                .ForMember(x => x.LocationAddress, x => x.MapFrom(y => FormatAddress(y.AcademicLocation)))
                .ForMember(x => x.OnlineUrl, x => x.MapFrom(y => y.OnlineMeetingUrl ?? (y.ModuleOffering != null ? y.ModuleOffering.OnlineClassroomUrl : null)))
                .ForMember(x => x.MeetingFormat, x => x.MapFrom(y => y.Format))
                .ForMember(x => x.AttendanceStatus, x => x.MapFrom(y => y.AttendanceRecords.Select(z => (AttendanceStatus?)z.Status).FirstOrDefault()))
                .ForMember(x => x.RelatedItemCount, x => x.MapFrom(y => y.PreparationAssignments.Count))
                .ForMember(x => x.AssessmentType, x => x.Ignore())
                .ForMember(x => x.AssignmentType, x => x.Ignore())
                .ForMember(x => x.AssignmentStatus, x => x.Ignore())
                .ForMember(x => x.IsRequired, x => x.Ignore())
                .ForMember(x => x.MaximumScore, x => x.Ignore())
                .ForMember(x => x.WeightPercentage, x => x.Ignore())
                .ForMember(x => x.ResultGrade, x => x.Ignore())
                .ForMember(x => x.ResultScore, x => x.Ignore())
                .ForMember(x => x.ResultPassed, x => x.Ignore());

            CreateMap<Assignment, StudentUpcomingEventModel>()
                .ForMember(x => x.EventType, x => x.MapFrom(_ => StudentUpcomingEventType.Assignment))
                .ForMember(x => x.StartsAt, x => x.MapFrom(y => y.DueAt!.Value))
                .ForMember(x => x.EndsAt, x => x.MapFrom(y => y.DueAt!.Value))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Instructions))
                .ForMember(x => x.ModuleId, x => x.MapFrom(y => y.ProgramModuleId))
                .ForMember(x => x.ModuleCode, x => x.MapFrom(y => y.ProgramModule != null ? y.ProgramModule.Code : null))
                .ForMember(x => x.ModuleName, x => x.MapFrom(y => y.ProgramModule != null ? y.ProgramModule.Name : null))
                .ForMember(x => x.MeetingTitle, x => x.MapFrom(y => y.Meeting != null ? y.Meeting.Title : null))
                .ForMember(x => x.AssignmentStatus, x => x.MapFrom(y => y.Status))
                .ForMember(x => x.IsRequired, x => x.MapFrom(y => y.IsPreparationRequired))
                .ForMember(x => x.ModuleOfferingId, x => x.Ignore())
                .ForMember(x => x.TeacherId, x => x.Ignore())
                .ForMember(x => x.TeacherName, x => x.Ignore())
                .ForMember(x => x.TeacherEmail, x => x.Ignore())
                .ForMember(x => x.LocationName, x => x.Ignore())
                .ForMember(x => x.LocationAddress, x => x.Ignore())
                .ForMember(x => x.OnlineUrl, x => x.Ignore())
                .ForMember(x => x.MeetingFormat, x => x.Ignore())
                .ForMember(x => x.AssessmentType, x => x.Ignore())
                .ForMember(x => x.AttendanceStatus, x => x.Ignore())
                .ForMember(x => x.ResultGrade, x => x.Ignore())
                .ForMember(x => x.ResultScore, x => x.Ignore())
                .ForMember(x => x.ResultPassed, x => x.Ignore())
                .ForMember(x => x.RelatedItemCount, x => x.Ignore());

            CreateMap<Exam, StudentUpcomingEventModel>()
                .ForMember(x => x.EventType, x => x.MapFrom(_ => StudentUpcomingEventType.Exam))
                .ForMember(x => x.StartsAt, x => x.MapFrom(y => y.ScheduledAt!.Value))
                .ForMember(x => x.EndsAt, x => x.MapFrom(y => y.DurationMinutes.HasValue ? y.ScheduledAt!.Value.AddMinutes(y.DurationMinutes.Value) : y.ScheduledAt!.Value))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Instructions))
                .ForMember(x => x.ModuleId, x => x.MapFrom(y => y.ProgramModuleId))
                .ForMember(x => x.ModuleCode, x => x.MapFrom(y => y.ProgramModule != null ? y.ProgramModule.Code : null))
                .ForMember(x => x.ModuleName, x => x.MapFrom(y => y.ProgramModule != null ? y.ProgramModule.Name : null))
                .ForMember(x => x.LocationName, x => x.MapFrom(y => y.Location))
                .ForMember(x => x.OnlineUrl, x => x.MapFrom(y => y.OnlineExamUrl))
                .ForMember(x => x.MaximumScore, x => x.MapFrom(y => y.PassingScore))
                .ForMember(x => x.ResultGrade, x => x.MapFrom(y => y.Results.Select(z => z.Grade).FirstOrDefault()))
                .ForMember(x => x.ResultScore, x => x.MapFrom(y => y.Results.Select(z => z.Score).FirstOrDefault()))
                .ForMember(x => x.ResultPassed, x => x.MapFrom(y => y.Results.Select(z => (bool?)z.Passed).FirstOrDefault()))
                .ForMember(x => x.ModuleOfferingId, x => x.Ignore())
                .ForMember(x => x.MeetingId, x => x.Ignore())
                .ForMember(x => x.MeetingTitle, x => x.Ignore())
                .ForMember(x => x.TeacherId, x => x.Ignore())
                .ForMember(x => x.TeacherName, x => x.Ignore())
                .ForMember(x => x.TeacherEmail, x => x.Ignore())
                .ForMember(x => x.LocationAddress, x => x.Ignore())
                .ForMember(x => x.MeetingFormat, x => x.Ignore())
                .ForMember(x => x.AssignmentType, x => x.Ignore())
                .ForMember(x => x.AssignmentStatus, x => x.Ignore())
                .ForMember(x => x.AttendanceStatus, x => x.Ignore())
                .ForMember(x => x.IsRequired, x => x.Ignore())
                .ForMember(x => x.RelatedItemCount, x => x.Ignore());

            CreateMap<Meeting, StudentMeetingDetailModel>()
                .ForMember(x => x.Summary, x => x.MapFrom(y => y))
                .ForMember(x => x.LearningObjectives, x => x.MapFrom(y => y.LearningObjectives.OrderBy(z => z.SortOrder)))
                .ForMember(x => x.LearningActivities, x => x.MapFrom(y => y.LearningActivities.OrderBy(z => z.SortOrder)))
                .ForMember(x => x.PreparationAssignments, x => x.MapFrom(y => y.PreparationAssignments.OrderBy(z => z.DueAt)))
                .ForMember(x => x.Resources, x => x.MapFrom(y => y.Resources.OrderBy(z => z.SortOrder)))
                .ForMember(x => x.LessonContents, x => x.MapFrom(y => y.LessonContents.OrderBy(z => z.SortOrder)))
                .ForMember(x => x.StudyTips, x => x.MapFrom(y => y.StudyTips.OrderBy(z => z.SortOrder)))
                .ForMember(x => x.Directions, x => x.MapFrom(y => y.AcademicLocation == null
                    ? Enumerable.Empty<LocationDirection>()
                    : y.AcademicLocation.Directions.OrderBy(z => z.SortOrder)));

            CreateMap<LearningObjective, LearningObjectiveModel>()
                .ConstructUsing(x => new LearningObjectiveModel(x.Id, x.ExternalId, x.Title, x.Description, x.BloomLevel, x.IsAssessed));
            CreateMap<LearningActivity, LearningActivityModel>()
                .ConstructUsing(x => new LearningActivityModel(x.Id, x.ExternalId, x.Title, x.Description, x.ActivityType, x.DurationMinutes, x.Instructions, x.IsRequired));
            CreateMap<Assignment, AssignmentSummaryModel>()
                .ConstructUsing(x => new AssignmentSummaryModel(x.Id, x.ExternalId, x.Title, x.AssignmentType, x.Status, x.DueAt, x.MaximumScore, x.WeightPercentage));
            CreateMap<LearningResource, ResourceSummaryModel>()
                .ConstructUsing(x => new ResourceSummaryModel(x.Id, x.ExternalId, x.Title, x.Description, x.ResourceType, x.Url, x.FileName, x.IsRequired, x.BibliographicReference == null ? null : x.BibliographicReference.CitationText));
            CreateMap<LessonContent, LessonContentSummaryModel>()
                .ConstructUsing(x => new LessonContentSummaryModel(x.Id, x.ExternalId, x.Title, x.Summary, x.EstimatedStudyMinutes, x.IsRequired));
            CreateMap<StudyTip, StudyTipSummaryModel>()
                .ConstructUsing(x => new StudyTipSummaryModel(x.Id, x.ExternalId, x.Title, x.Body, x.Category, x.IsHighlighted));
            CreateMap<LocationDirection, LocationDirectionModel>()
                .ConstructUsing(x => new LocationDirectionModel(x.TravelMode, x.Title, x.Instructions, x.PublicTransportStop, x.ParkingInstructions, x.ExternalNavigationUrl));
        }

        private static string? FormatAddress(AcademicLocation? location)
        {
            if (location is null)
            {
                return null;
            }

            var parts = new[]
            {
                location.AddressLine1,
                location.AddressLine2,
                location.PostalCode,
                location.City,
                location.Country
            };

            var address = string.Join(", ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));

            return string.IsNullOrWhiteSpace(address) ? null : address;
        }
    }
}
