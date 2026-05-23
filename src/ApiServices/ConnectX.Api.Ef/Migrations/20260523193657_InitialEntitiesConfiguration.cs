using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectX.Api.Ef.Migrations
{
    /// <inheritdoc />
    public partial class InitialEntitiesConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "AcademicLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LocationType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    City = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Country = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RoomNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    BuildingName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ReceptionPhoneNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    MapUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    IsAccessible = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformAccessLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeviceType = table.Column<int>(type: "integer", nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    BrowserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OperatingSystem = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Country = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    WasSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformAccessLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentPersonalFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    City = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Country = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    EmergencyContactName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    LastConfirmedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPersonalFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CreditValue = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    NominalStudyHours = table.Column<int>(type: "integer", nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: true),
                    EndsOn = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StaffNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Biography = table.Column<string>(type: "text", nullable: true),
                    ExpertiseArea = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OfficeLocation = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PreferredContactMethod = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsFreelance = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationDirections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    AcademicLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TravelMode = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false),
                    PublicTransportStop = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ParkingInstructions = table.Column<string>(type: "text", nullable: true),
                    AccessibilityNotes = table.Column<string>(type: "text", nullable: true),
                    ExternalNavigationUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationDirections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationDirections_AcademicLocations_AcademicLocationId",
                        column: x => x.AcademicLocationId,
                        principalTable: "AcademicLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetailChangeRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentPersonalFileId = table.Column<Guid>(type: "uuid", nullable: true),
                    FieldName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CurrentValue = table.Column<string>(type: "text", nullable: true),
                    RequestedValue = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReviewedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReviewedByStaffId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewerNotes = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetailChangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDetailChangeRequests_StudentPersonalFiles_StudentPe~",
                        column: x => x.StudentPersonalFileId,
                        principalTable: "StudentPersonalFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cohorts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudyProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AcademicYear = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    EndsOn = table.Column<DateOnly>(type: "date", nullable: true),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DeliveryMode = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cohorts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cohorts_StudyPrograms_StudyProgramId",
                        column: x => x.StudyProgramId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudyProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EnrolledOn = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpectedCompletionOn = table.Column<DateOnly>(type: "date", nullable: true),
                    CompletedOn = table.Column<DateOnly>(type: "date", nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    AdvisorNotes = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramEnrollments_StudyPrograms_StudyProgramId",
                        column: x => x.StudyProgramId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudyProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoordinatorTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SequenceNumber = table.Column<int>(type: "integer", nullable: false),
                    CreditValue = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    EstimatedStudyHours = table.Column<int>(type: "integer", nullable: false),
                    Prerequisites = table.Column<string>(type: "text", nullable: true),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramModules_StudyPrograms_StudyProgramId",
                        column: x => x.StudyProgramId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramModules_Teachers_CoordinatorTeacherId",
                        column: x => x.CoordinatorTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DownloadDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudyProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublishedByTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    FileUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Version = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownloadDocuments_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DownloadDocuments_StudyPrograms_StudyProgramId",
                        column: x => x.StudyProgramId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DownloadDocuments_Teachers_PublishedByTeacherId",
                        column: x => x.PublishedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AssessmentType = table.Column<int>(type: "integer", nullable: false),
                    ScheduledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OnlineExamUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    RegistrationDeadline = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WeightPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    PassingScore = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    Instructions = table.Column<string>(type: "text", nullable: true),
                    ResultsPublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamTrainings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OpensAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosesAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TimeLimitMinutes = table.Column<int>(type: "integer", nullable: true),
                    PassingScore = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    IsOptional = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamTrainings_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleOfferings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CohortId = table.Column<Guid>(type: "uuid", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcademicLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcademicYear = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Term = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DeliveryMode = table.Column<int>(type: "integer", nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    EndsOn = table.Column<DateOnly>(type: "date", nullable: true),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OnlineClassroomUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    MaximumParticipants = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleOfferings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleOfferings_AcademicLocations_AcademicLocationId",
                        column: x => x.AcademicLocationId,
                        principalTable: "AcademicLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleOfferings_Cohorts_CohortId",
                        column: x => x.CohortId,
                        principalTable: "Cohorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleOfferings_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleOfferings_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    Grade = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Passed = table.Column<bool>(type: "boolean", nullable: false),
                    RecordedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Feedback = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamResults_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ModuleOfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Audience = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsPinned = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_ModuleOfferings_ModuleOfferingId",
                        column: x => x.ModuleOfferingId,
                        principalTable: "ModuleOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Announcements_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ModuleOfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademicLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SequenceNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Format = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OnlineMeetingUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    PreparationInstructions = table.Column<string>(type: "text", nullable: true),
                    CancellationReason = table.Column<string>(type: "text", nullable: true),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_AcademicLocations_AcademicLocationId",
                        column: x => x.AcademicLocationId,
                        principalTable: "AcademicLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Meetings_ModuleOfferings_ModuleOfferingId",
                        column: x => x.ModuleOfferingId,
                        principalTable: "ModuleOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModuleOfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EnrolledOn = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletedOn = table.Column<DateOnly>(type: "date", nullable: true),
                    FinalGrade = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    AttendancePercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    CompletionRemarks = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleEnrollments_ModuleOfferings_ModuleOfferingId",
                        column: x => x.ModuleOfferingId,
                        principalTable: "ModuleOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModuleOfferingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReadAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ActionUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentNotifications_ModuleOfferings_ModuleOfferingId",
                        column: x => x.ModuleOfferingId,
                        principalTable: "ModuleOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModuleOfferingId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Subject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastResponseAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AssignedStaffId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportRequests_ModuleOfferings_ModuleOfferingId",
                        column: x => x.ModuleOfferingId,
                        principalTable: "ModuleOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportRequests_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: true),
                    AssignmentType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AvailableFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DueAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MaximumScore = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    WeightPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    IsPreparationRequired = table.Column<bool>(type: "boolean", nullable: false),
                    AllowsResubmission = table.Column<bool>(type: "boolean", nullable: false),
                    RubricUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CheckedInAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CheckedOutAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BibliographicReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceType = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Authors = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Editor = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Publisher = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    JournalName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Edition = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Volume = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Issue = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    PageRange = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    PublicationYear = table.Column<int>(type: "integer", nullable: true),
                    Isbn = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Issn = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Doi = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CitationText = table.Column<string>(type: "text", nullable: true),
                    IsRequiredReading = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibliographicReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BibliographicReferences_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibliographicReferences_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ModuleOfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByRole = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: true),
                    Audience = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsPinned = table.Column<bool>(type: "boolean", nullable: false),
                    OpenedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ClosedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionTopics_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscussionTopics_ModuleOfferings_ModuleOfferingId",
                        column: x => x.ModuleOfferingId,
                        principalTable: "ModuleOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InteractiveApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LaunchUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ProviderName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OpensInNewWindow = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresAuthentication = table.Column<bool>(type: "boolean", nullable: false),
                    TracksProgress = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractiveApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteractiveApplications_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InteractiveApplications_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ActivityType = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    Instructions = table.Column<string>(type: "text", nullable: true),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningActivities_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningObjectives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    BloomLevel = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsAssessed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningObjectives_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningObjectives_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessonContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublishedByTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    EstimatedStudyMinutes = table.Column<int>(type: "integer", nullable: true),
                    AvailableFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Visibility = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonContents_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonContents_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonContents_Teachers_PublishedByTeacherId",
                        column: x => x.PublishedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OnlineTests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: true),
                    IsPracticeTest = table.Column<bool>(type: "boolean", nullable: false),
                    OpensAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosesAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TimeLimitMinutes = table.Column<int>(type: "integer", nullable: true),
                    AttemptLimit = table.Column<int>(type: "integer", nullable: true),
                    PassingScore = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineTests_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnlineTests_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PracticeExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: true),
                    DifficultyLevel = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    EstimatedDurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    IsOptional = table.Column<bool>(type: "boolean", nullable: false),
                    ResourceUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    SolutionUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticeExercises_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PracticeExercises_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudyTips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublishedByTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsHighlighted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyTips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyTips_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudyTips_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudyTips_Teachers_PublishedByTeacherId",
                        column: x => x.PublishedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplementaryMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublishedByTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ResourceType = table.Column<int>(type: "integer", nullable: false),
                    FileUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ExternalUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsHighlighted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplementaryMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplementaryMaterials_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplementaryMaterials_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplementaryMaterials_Teachers_PublishedByTeacherId",
                        column: x => x.PublishedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    SupportRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorRole = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    SentAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsInternalNote = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportMessages_SupportRequests_SupportRequestId",
                        column: x => x.SupportRequestId,
                        principalTable: "SupportRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedByTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SubmissionText = table.Column<string>(type: "text", nullable: true),
                    FileUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Score = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    Grade = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Feedback = table.Column<string>(type: "text", nullable: true),
                    GradedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Teachers_GradedByTeacherId",
                        column: x => x.GradedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ProgramModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublishedByTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    BibliographicReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ResourceType = table.Column<int>(type: "integer", nullable: false),
                    Visibility = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    FileName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningResources_BibliographicReferences_BibliographicRefe~",
                        column: x => x.BibliographicReferenceId,
                        principalTable: "BibliographicReferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningResources_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningResources_ProgramModules_ProgramModuleId",
                        column: x => x.ProgramModuleId,
                        principalTable: "ProgramModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningResources_Teachers_PublishedByTeacherId",
                        column: x => x.PublishedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    DiscussionTopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentPostId = table.Column<Guid>(type: "uuid", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorRole = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    PostedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EditedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsInstructorEndorsed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionPosts_DiscussionPosts_ParentPostId",
                        column: x => x.ParentPostId,
                        principalTable: "DiscussionPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscussionPosts_DiscussionTopics_DiscussionTopicId",
                        column: x => x.DiscussionTopicId,
                        principalTable: "DiscussionTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamTrainingPracticeTests",
                columns: table => new
                {
                    ExamTrainingId = table.Column<Guid>(type: "uuid", nullable: false),
                    OnlineTestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamTrainingPracticeTests", x => new { x.ExamTrainingId, x.OnlineTestId });
                    table.ForeignKey(
                        name: "FK_ExamTrainingPracticeTests_ExamTrainings_ExamTrainingId",
                        column: x => x.ExamTrainingId,
                        principalTable: "ExamTrainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamTrainingPracticeTests_OnlineTests_OnlineTestId",
                        column: x => x.OnlineTestId,
                        principalTable: "OnlineTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    OnlineTestId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    Passed = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAttempts_OnlineTests_OnlineTestId",
                        column: x => x.OnlineTestId,
                        principalTable: "OnlineTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    OnlineTestId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionType = table.Column<int>(type: "integer", nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    Explanation = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestQuestions_OnlineTests_OnlineTestId",
                        column: x => x.OnlineTestId,
                        principalTable: "OnlineTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamTrainingPracticeExercises",
                columns: table => new
                {
                    ExamTrainingId = table.Column<Guid>(type: "uuid", nullable: false),
                    PracticeExerciseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamTrainingPracticeExercises", x => new { x.ExamTrainingId, x.PracticeExerciseId });
                    table.ForeignKey(
                        name: "FK_ExamTrainingPracticeExercises_ExamTrainings_ExamTrainingId",
                        column: x => x.ExamTrainingId,
                        principalTable: "ExamTrainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamTrainingPracticeExercises_PracticeExercises_PracticeExe~",
                        column: x => x.PracticeExerciseId,
                        principalTable: "PracticeExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    TestQuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Feedback = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestOptions_TestQuestions_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalTable: "TestQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    TestAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestQuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedOptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResponseText = table.Column<string>(type: "text", nullable: true),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: true),
                    PointsAwarded = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAnswers_TestAttempts_TestAttemptId",
                        column: x => x.TestAttemptId,
                        principalTable: "TestAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestAnswers_TestOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "TestOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestAnswers_TestQuestions_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalTable: "TestQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AcademicQuestionReplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    AcademicQuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorRole = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    PostedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsAcceptedAnswer = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicQuestionReplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcademicQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModuleOfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PracticeExerciseId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AskedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AcceptedAnswerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicQuestions_AcademicQuestionReplies_AcceptedAnswerId",
                        column: x => x.AcceptedAnswerId,
                        principalTable: "AcademicQuestionReplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicQuestions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicQuestions_ModuleOfferings_ModuleOfferingId",
                        column: x => x.ModuleOfferingId,
                        principalTable: "ModuleOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicQuestions_PracticeExercises_PracticeExerciseId",
                        column: x => x.PracticeExerciseId,
                        principalTable: "PracticeExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicLocations_Code",
                table: "AcademicLocations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicLocations_ExternalId",
                table: "AcademicLocations",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicLocations_IsActive_City",
                table: "AcademicLocations",
                columns: new[] { "IsActive", "City" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicLocations_IsDeleted_UpdatedDate",
                table: "AcademicLocations",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicLocations_Latitude_Longitude",
                table: "AcademicLocations",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestionReplies_AcademicQuestionId_PostedAt",
                table: "AcademicQuestionReplies",
                columns: new[] { "AcademicQuestionId", "PostedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestionReplies_AuthorId_AuthorRole",
                table: "AcademicQuestionReplies",
                columns: new[] { "AuthorId", "AuthorRole" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestionReplies_ExternalId",
                table: "AcademicQuestionReplies",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestionReplies_IsDeleted_UpdatedDate",
                table: "AcademicQuestionReplies",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestions_AcceptedAnswerId",
                table: "AcademicQuestions",
                column: "AcceptedAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestions_AssignmentId",
                table: "AcademicQuestions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestions_ExternalId",
                table: "AcademicQuestions",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestions_IsDeleted_UpdatedDate",
                table: "AcademicQuestions",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestions_ModuleOfferingId_Status",
                table: "AcademicQuestions",
                columns: new[] { "ModuleOfferingId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestions_PracticeExerciseId",
                table: "AcademicQuestions",
                column: "PracticeExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicQuestions_StudentId",
                table: "AcademicQuestions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ExpiresAt",
                table: "Announcements",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ExternalId",
                table: "Announcements",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_IsDeleted_UpdatedDate",
                table: "Announcements",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ModuleOfferingId_IsPinned",
                table: "Announcements",
                columns: new[] { "ModuleOfferingId", "IsPinned" });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ModuleOfferingId_PublishedAt",
                table: "Announcements",
                columns: new[] { "ModuleOfferingId", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TeacherId",
                table: "Announcements",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DueAt",
                table: "Assignments",
                column: "DueAt");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ExternalId",
                table: "Assignments",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_IsDeleted_UpdatedDate",
                table: "Assignments",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_MeetingId_DueAt",
                table: "Assignments",
                columns: new[] { "MeetingId", "DueAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ProgramModuleId_Status",
                table: "Assignments",
                columns: new[] { "ProgramModuleId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_AssignmentId_Status",
                table: "AssignmentSubmissions",
                columns: new[] { "AssignmentId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_AssignmentId_StudentId_AttemptNumber",
                table: "AssignmentSubmissions",
                columns: new[] { "AssignmentId", "StudentId", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_ExternalId",
                table: "AssignmentSubmissions",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_GradedByTeacherId",
                table: "AssignmentSubmissions",
                column: "GradedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_IsDeleted_UpdatedDate",
                table: "AssignmentSubmissions",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_StudentId",
                table: "AssignmentSubmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_ExternalId",
                table: "AttendanceRecords",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_IsDeleted_UpdatedDate",
                table: "AttendanceRecords",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_MeetingId_Status",
                table: "AttendanceRecords",
                columns: new[] { "MeetingId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_MeetingId_StudentId",
                table: "AttendanceRecords",
                columns: new[] { "MeetingId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentId",
                table: "AttendanceRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_BibliographicReferences_Doi",
                table: "BibliographicReferences",
                column: "Doi");

            migrationBuilder.CreateIndex(
                name: "IX_BibliographicReferences_ExternalId",
                table: "BibliographicReferences",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BibliographicReferences_Isbn",
                table: "BibliographicReferences",
                column: "Isbn");

            migrationBuilder.CreateIndex(
                name: "IX_BibliographicReferences_IsDeleted_UpdatedDate",
                table: "BibliographicReferences",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BibliographicReferences_MeetingId",
                table: "BibliographicReferences",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_BibliographicReferences_ProgramModuleId_ReferenceType",
                table: "BibliographicReferences",
                columns: new[] { "ProgramModuleId", "ReferenceType" });

            migrationBuilder.CreateIndex(
                name: "IX_BibliographicReferences_ProgramModuleId_SortOrder",
                table: "BibliographicReferences",
                columns: new[] { "ProgramModuleId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Cohorts_ExternalId",
                table: "Cohorts",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cohorts_IsDeleted_UpdatedDate",
                table: "Cohorts",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Cohorts_StudyProgramId_AcademicYear",
                table: "Cohorts",
                columns: new[] { "StudyProgramId", "AcademicYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Cohorts_StudyProgramId_Name",
                table: "Cohorts",
                columns: new[] { "StudyProgramId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionPosts_AuthorId_AuthorRole",
                table: "DiscussionPosts",
                columns: new[] { "AuthorId", "AuthorRole" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionPosts_DiscussionTopicId_PostedAt",
                table: "DiscussionPosts",
                columns: new[] { "DiscussionTopicId", "PostedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionPosts_ExternalId",
                table: "DiscussionPosts",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionPosts_IsDeleted_UpdatedDate",
                table: "DiscussionPosts",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionPosts_ParentPostId",
                table: "DiscussionPosts",
                column: "ParentPostId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionTopics_CreatedById_CreatedByRole",
                table: "DiscussionTopics",
                columns: new[] { "CreatedById", "CreatedByRole" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionTopics_ExternalId",
                table: "DiscussionTopics",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionTopics_IsDeleted_UpdatedDate",
                table: "DiscussionTopics",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionTopics_MeetingId",
                table: "DiscussionTopics",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionTopics_ModuleOfferingId_OpenedAt",
                table: "DiscussionTopics",
                columns: new[] { "ModuleOfferingId", "OpenedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionTopics_ModuleOfferingId_Status_IsPinned",
                table: "DiscussionTopics",
                columns: new[] { "ModuleOfferingId", "Status", "IsPinned" });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadDocuments_ExpiresAt",
                table: "DownloadDocuments",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadDocuments_ExternalId",
                table: "DownloadDocuments",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DownloadDocuments_IsDeleted_UpdatedDate",
                table: "DownloadDocuments",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadDocuments_ProgramModuleId_DocumentType",
                table: "DownloadDocuments",
                columns: new[] { "ProgramModuleId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadDocuments_PublishedByTeacherId",
                table: "DownloadDocuments",
                column: "PublishedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadDocuments_StudyProgramId_DocumentType",
                table: "DownloadDocuments",
                columns: new[] { "StudyProgramId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId_StudentId_AttemptNumber",
                table: "ExamResults",
                columns: new[] { "ExamId", "StudentId", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExternalId",
                table: "ExamResults",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_IsDeleted_UpdatedDate",
                table: "ExamResults",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_StudentId",
                table: "ExamResults",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_StudentId_PublishedAt",
                table: "ExamResults",
                columns: new[] { "StudentId", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExternalId",
                table: "Exams",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_IsDeleted_UpdatedDate",
                table: "Exams",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ProgramModuleId_ScheduledAt",
                table: "Exams",
                columns: new[] { "ProgramModuleId", "ScheduledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ResultsPublishedAt",
                table: "Exams",
                column: "ResultsPublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ExamTrainingPracticeExercises_PracticeExerciseId",
                table: "ExamTrainingPracticeExercises",
                column: "PracticeExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamTrainingPracticeTests_OnlineTestId",
                table: "ExamTrainingPracticeTests",
                column: "OnlineTestId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamTrainings_ClosesAt",
                table: "ExamTrainings",
                column: "ClosesAt");

            migrationBuilder.CreateIndex(
                name: "IX_ExamTrainings_ExternalId",
                table: "ExamTrainings",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamTrainings_IsDeleted_UpdatedDate",
                table: "ExamTrainings",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamTrainings_ProgramModuleId_OpensAt",
                table: "ExamTrainings",
                columns: new[] { "ProgramModuleId", "OpensAt" });

            migrationBuilder.CreateIndex(
                name: "IX_InteractiveApplications_ExternalId",
                table: "InteractiveApplications",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteractiveApplications_IsDeleted_UpdatedDate",
                table: "InteractiveApplications",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_InteractiveApplications_MeetingId",
                table: "InteractiveApplications",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractiveApplications_ProgramModuleId",
                table: "InteractiveApplications",
                column: "ProgramModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningActivities_ExternalId",
                table: "LearningActivities",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningActivities_IsDeleted_UpdatedDate",
                table: "LearningActivities",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningActivities_MeetingId_ActivityType",
                table: "LearningActivities",
                columns: new[] { "MeetingId", "ActivityType" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningActivities_MeetingId_SortOrder",
                table: "LearningActivities",
                columns: new[] { "MeetingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjectives_ExternalId",
                table: "LearningObjectives",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjectives_IsDeleted_UpdatedDate",
                table: "LearningObjectives",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjectives_MeetingId_SortOrder",
                table: "LearningObjectives",
                columns: new[] { "MeetingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjectives_ProgramModuleId_SortOrder",
                table: "LearningObjectives",
                columns: new[] { "ProgramModuleId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_BibliographicReferenceId",
                table: "LearningResources",
                column: "BibliographicReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_ExternalId",
                table: "LearningResources",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_IsDeleted_UpdatedDate",
                table: "LearningResources",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_MeetingId_SortOrder",
                table: "LearningResources",
                columns: new[] { "MeetingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_ProgramModuleId_ResourceType",
                table: "LearningResources",
                columns: new[] { "ProgramModuleId", "ResourceType" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_ProgramModuleId_SortOrder",
                table: "LearningResources",
                columns: new[] { "ProgramModuleId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_PublishedByTeacherId",
                table: "LearningResources",
                column: "PublishedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonContents_AvailableFrom",
                table: "LessonContents",
                column: "AvailableFrom");

            migrationBuilder.CreateIndex(
                name: "IX_LessonContents_ExternalId",
                table: "LessonContents",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonContents_IsDeleted_UpdatedDate",
                table: "LessonContents",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonContents_MeetingId_SortOrder",
                table: "LessonContents",
                columns: new[] { "MeetingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonContents_ProgramModuleId_SortOrder",
                table: "LessonContents",
                columns: new[] { "ProgramModuleId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonContents_PublishedByTeacherId",
                table: "LessonContents",
                column: "PublishedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationDirections_AcademicLocationId_SortOrder",
                table: "LocationDirections",
                columns: new[] { "AcademicLocationId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LocationDirections_AcademicLocationId_TravelMode",
                table: "LocationDirections",
                columns: new[] { "AcademicLocationId", "TravelMode" });

            migrationBuilder.CreateIndex(
                name: "IX_LocationDirections_ExternalId",
                table: "LocationDirections",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationDirections_IsDeleted_UpdatedDate",
                table: "LocationDirections",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_AcademicLocationId",
                table: "Meetings",
                column: "AcademicLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ExternalId",
                table: "Meetings",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_IsDeleted_UpdatedDate",
                table: "Meetings",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ModuleOfferingId_SequenceNumber",
                table: "Meetings",
                columns: new[] { "ModuleOfferingId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ModuleOfferingId_StartsAt",
                table: "Meetings",
                columns: new[] { "ModuleOfferingId", "StartsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleEnrollments_ExternalId",
                table: "ModuleEnrollments",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleEnrollments_IsDeleted_UpdatedDate",
                table: "ModuleEnrollments",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleEnrollments_ModuleOfferingId_Status",
                table: "ModuleEnrollments",
                columns: new[] { "ModuleOfferingId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleEnrollments_StudentId",
                table: "ModuleEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleEnrollments_StudentId_ModuleOfferingId",
                table: "ModuleEnrollments",
                columns: new[] { "StudentId", "ModuleOfferingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOfferings_AcademicLocationId",
                table: "ModuleOfferings",
                column: "AcademicLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOfferings_CohortId_StartsOn",
                table: "ModuleOfferings",
                columns: new[] { "CohortId", "StartsOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOfferings_ExternalId",
                table: "ModuleOfferings",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOfferings_IsDeleted_UpdatedDate",
                table: "ModuleOfferings",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOfferings_ProgramModuleId_AcademicYear_Term",
                table: "ModuleOfferings",
                columns: new[] { "ProgramModuleId", "AcademicYear", "Term" });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleOfferings_TeacherId_StartsOn",
                table: "ModuleOfferings",
                columns: new[] { "TeacherId", "StartsOn" });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineTests_ClosesAt",
                table: "OnlineTests",
                column: "ClosesAt");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineTests_ExternalId",
                table: "OnlineTests",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineTests_IsDeleted_UpdatedDate",
                table: "OnlineTests",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineTests_MeetingId_OpensAt",
                table: "OnlineTests",
                columns: new[] { "MeetingId", "OpensAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineTests_ProgramModuleId_IsPracticeTest",
                table: "OnlineTests",
                columns: new[] { "ProgramModuleId", "IsPracticeTest" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetailChangeRequests_ExternalId",
                table: "PersonalDetailChangeRequests",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetailChangeRequests_IsDeleted_UpdatedDate",
                table: "PersonalDetailChangeRequests",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetailChangeRequests_ReviewedByStaffId",
                table: "PersonalDetailChangeRequests",
                column: "ReviewedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetailChangeRequests_StudentId_Status",
                table: "PersonalDetailChangeRequests",
                columns: new[] { "StudentId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetailChangeRequests_StudentPersonalFileId_Submitte~",
                table: "PersonalDetailChangeRequests",
                columns: new[] { "StudentPersonalFileId", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAccessLogs_ExternalId",
                table: "PlatformAccessLogs",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAccessLogs_IsDeleted_UpdatedDate",
                table: "PlatformAccessLogs",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAccessLogs_StudentId_AccessedAt",
                table: "PlatformAccessLogs",
                columns: new[] { "StudentId", "AccessedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAccessLogs_StudentId_WasSuccessful",
                table: "PlatformAccessLogs",
                columns: new[] { "StudentId", "WasSuccessful" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeExercises_ExternalId",
                table: "PracticeExercises",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PracticeExercises_IsDeleted_UpdatedDate",
                table: "PracticeExercises",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeExercises_MeetingId",
                table: "PracticeExercises",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeExercises_ProgramModuleId_IsOptional",
                table: "PracticeExercises",
                columns: new[] { "ProgramModuleId", "IsOptional" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_ExternalId",
                table: "ProgramEnrollments",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_IsDeleted_UpdatedDate",
                table: "ProgramEnrollments",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_StudentId",
                table: "ProgramEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_StudentId_StudyProgramId",
                table: "ProgramEnrollments",
                columns: new[] { "StudentId", "StudyProgramId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollments_StudyProgramId_Status",
                table: "ProgramEnrollments",
                columns: new[] { "StudyProgramId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModules_CoordinatorTeacherId",
                table: "ProgramModules",
                column: "CoordinatorTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModules_ExternalId",
                table: "ProgramModules",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModules_IsDeleted_UpdatedDate",
                table: "ProgramModules",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModules_StudyProgramId_Code",
                table: "ProgramModules",
                columns: new[] { "StudyProgramId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModules_StudyProgramId_SequenceNumber",
                table: "ProgramModules",
                columns: new[] { "StudyProgramId", "SequenceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotifications_ExternalId",
                table: "StudentNotifications",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotifications_IsDeleted_UpdatedDate",
                table: "StudentNotifications",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotifications_ModuleOfferingId",
                table: "StudentNotifications",
                column: "ModuleOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotifications_StudentId_CreatedAt",
                table: "StudentNotifications",
                columns: new[] { "StudentId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotifications_StudentId_ReadAt",
                table: "StudentNotifications",
                columns: new[] { "StudentId", "ReadAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonalFiles_Email",
                table: "StudentPersonalFiles",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonalFiles_ExternalId",
                table: "StudentPersonalFiles",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonalFiles_IsDeleted_UpdatedDate",
                table: "StudentPersonalFiles",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonalFiles_LastName_FirstName",
                table: "StudentPersonalFiles",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonalFiles_StudentId",
                table: "StudentPersonalFiles",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonalFiles_StudentNumber",
                table: "StudentPersonalFiles",
                column: "StudentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPrograms_Code",
                table: "StudyPrograms",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPrograms_ExternalId",
                table: "StudyPrograms",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPrograms_IsActive_Level",
                table: "StudyPrograms",
                columns: new[] { "IsActive", "Level" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPrograms_IsDeleted_UpdatedDate",
                table: "StudyPrograms",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyTips_ExternalId",
                table: "StudyTips",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyTips_IsDeleted_UpdatedDate",
                table: "StudyTips",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyTips_MeetingId_SortOrder",
                table: "StudyTips",
                columns: new[] { "MeetingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyTips_ProgramModuleId_Category",
                table: "StudyTips",
                columns: new[] { "ProgramModuleId", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyTips_PublishedByTeacherId",
                table: "StudyTips",
                column: "PublishedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplementaryMaterials_ExternalId",
                table: "SupplementaryMaterials",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplementaryMaterials_IsDeleted_UpdatedDate",
                table: "SupplementaryMaterials",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SupplementaryMaterials_MeetingId_PublishedAt",
                table: "SupplementaryMaterials",
                columns: new[] { "MeetingId", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SupplementaryMaterials_ProgramModuleId_PublishedAt",
                table: "SupplementaryMaterials",
                columns: new[] { "ProgramModuleId", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SupplementaryMaterials_PublishedByTeacherId",
                table: "SupplementaryMaterials",
                column: "PublishedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_AuthorId_AuthorRole",
                table: "SupportMessages",
                columns: new[] { "AuthorId", "AuthorRole" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_ExternalId",
                table: "SupportMessages",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_IsDeleted_UpdatedDate",
                table: "SupportMessages",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_SupportRequestId_SentAt",
                table: "SupportMessages",
                columns: new[] { "SupportRequestId", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_AssignedStaffId",
                table: "SupportRequests",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_ExternalId",
                table: "SupportRequests",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_IsDeleted_UpdatedDate",
                table: "SupportRequests",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_ModuleOfferingId",
                table: "SupportRequests",
                column: "ModuleOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_ProgramModuleId",
                table: "SupportRequests",
                column: "ProgramModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_ReferenceNumber",
                table: "SupportRequests",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_Status_Priority",
                table: "SupportRequests",
                columns: new[] { "Status", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_StudentId_Status",
                table: "SupportRequests",
                columns: new[] { "StudentId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ExternalId",
                table: "Teachers",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_IsActive_LastName",
                table: "Teachers",
                columns: new[] { "IsActive", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_IsDeleted_UpdatedDate",
                table: "Teachers",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_StaffNumber",
                table: "Teachers",
                column: "StaffNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_ExternalId",
                table: "TestAnswers",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_IsDeleted_UpdatedDate",
                table: "TestAnswers",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_SelectedOptionId",
                table: "TestAnswers",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_TestAttemptId_TestQuestionId",
                table: "TestAnswers",
                columns: new[] { "TestAttemptId", "TestQuestionId" });

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_TestQuestionId",
                table: "TestAnswers",
                column: "TestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_ExternalId",
                table: "TestAttempts",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_IsDeleted_UpdatedDate",
                table: "TestAttempts",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_OnlineTestId_StudentId_AttemptNumber",
                table: "TestAttempts",
                columns: new[] { "OnlineTestId", "StudentId", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_StudentId",
                table: "TestAttempts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_StudentId_StartedAt",
                table: "TestAttempts",
                columns: new[] { "StudentId", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TestOptions_ExternalId",
                table: "TestOptions",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestOptions_IsDeleted_UpdatedDate",
                table: "TestOptions",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TestOptions_TestQuestionId_SortOrder",
                table: "TestOptions",
                columns: new[] { "TestQuestionId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_ExternalId",
                table: "TestQuestions",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_IsDeleted_UpdatedDate",
                table: "TestQuestions",
                columns: new[] { "IsDeleted", "UpdatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_OnlineTestId_SortOrder",
                table: "TestQuestions",
                columns: new[] { "OnlineTestId", "SortOrder" });

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicQuestionReplies_AcademicQuestions_AcademicQuestionId",
                table: "AcademicQuestionReplies",
                column: "AcademicQuestionId",
                principalTable: "AcademicQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicQuestionReplies_AcademicQuestions_AcademicQuestionId",
                table: "AcademicQuestionReplies");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "AssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "DiscussionPosts");

            migrationBuilder.DropTable(
                name: "DownloadDocuments");

            migrationBuilder.DropTable(
                name: "ExamResults");

            migrationBuilder.DropTable(
                name: "ExamTrainingPracticeExercises");

            migrationBuilder.DropTable(
                name: "ExamTrainingPracticeTests");

            migrationBuilder.DropTable(
                name: "InteractiveApplications");

            migrationBuilder.DropTable(
                name: "LearningActivities");

            migrationBuilder.DropTable(
                name: "LearningObjectives");

            migrationBuilder.DropTable(
                name: "LearningResources");

            migrationBuilder.DropTable(
                name: "LessonContents");

            migrationBuilder.DropTable(
                name: "LocationDirections");

            migrationBuilder.DropTable(
                name: "ModuleEnrollments");

            migrationBuilder.DropTable(
                name: "PersonalDetailChangeRequests");

            migrationBuilder.DropTable(
                name: "PlatformAccessLogs");

            migrationBuilder.DropTable(
                name: "ProgramEnrollments");

            migrationBuilder.DropTable(
                name: "StudentNotifications");

            migrationBuilder.DropTable(
                name: "StudyTips");

            migrationBuilder.DropTable(
                name: "SupplementaryMaterials");

            migrationBuilder.DropTable(
                name: "SupportMessages");

            migrationBuilder.DropTable(
                name: "TestAnswers");

            migrationBuilder.DropTable(
                name: "DiscussionTopics");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "ExamTrainings");

            migrationBuilder.DropTable(
                name: "BibliographicReferences");

            migrationBuilder.DropTable(
                name: "StudentPersonalFiles");

            migrationBuilder.DropTable(
                name: "SupportRequests");

            migrationBuilder.DropTable(
                name: "TestAttempts");

            migrationBuilder.DropTable(
                name: "TestOptions");

            migrationBuilder.DropTable(
                name: "TestQuestions");

            migrationBuilder.DropTable(
                name: "OnlineTests");

            migrationBuilder.DropTable(
                name: "AcademicQuestions");

            migrationBuilder.DropTable(
                name: "AcademicQuestionReplies");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "PracticeExercises");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "ModuleOfferings");

            migrationBuilder.DropTable(
                name: "AcademicLocations");

            migrationBuilder.DropTable(
                name: "Cohorts");

            migrationBuilder.DropTable(
                name: "ProgramModules");

            migrationBuilder.DropTable(
                name: "StudyPrograms");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
