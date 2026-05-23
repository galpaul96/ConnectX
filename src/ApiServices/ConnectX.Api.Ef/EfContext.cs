using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace ConnectX.Api.Ef
{
    internal class EfContext : DbContext
    {
        public DbSet<StudyProgram> StudyPrograms => Set<StudyProgram>();
        public DbSet<ProgramModule> ProgramModules => Set<ProgramModule>();
        public DbSet<Cohort> Cohorts => Set<Cohort>();
        public DbSet<ModuleOffering> ModuleOfferings => Set<ModuleOffering>();
        public DbSet<ProgramEnrollment> ProgramEnrollments => Set<ProgramEnrollment>();
        public DbSet<ModuleEnrollment> ModuleEnrollments => Set<ModuleEnrollment>();
        public DbSet<Teacher> Teachers => Set<Teacher>();

        public DbSet<Meeting> Meetings => Set<Meeting>();
        public DbSet<LearningObjective> LearningObjectives => Set<LearningObjective>();
        public DbSet<LearningActivity> LearningActivities => Set<LearningActivity>();
        public DbSet<LessonContent> LessonContents => Set<LessonContent>();
        public DbSet<StudyTip> StudyTips => Set<StudyTip>();
        public DbSet<PracticeExercise> PracticeExercises => Set<PracticeExercise>();
        public DbSet<ExamTraining> ExamTrainings => Set<ExamTraining>();
        public DbSet<InteractiveApplication> InteractiveApplications => Set<InteractiveApplication>();
        public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();

        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<AssignmentSubmission> AssignmentSubmissions => Set<AssignmentSubmission>();
        public DbSet<Exam> Exams => Set<Exam>();
        public DbSet<ExamResult> ExamResults => Set<ExamResult>();
        public DbSet<OnlineTest> OnlineTests => Set<OnlineTest>();
        public DbSet<TestQuestion> TestQuestions => Set<TestQuestion>();
        public DbSet<TestOption> TestOptions => Set<TestOption>();
        public DbSet<TestAttempt> TestAttempts => Set<TestAttempt>();
        public DbSet<TestAnswer> TestAnswers => Set<TestAnswer>();

        public DbSet<LearningResource> LearningResources => Set<LearningResource>();
        public DbSet<DownloadDocument> DownloadDocuments => Set<DownloadDocument>();
        public DbSet<SupplementaryMaterial> SupplementaryMaterials => Set<SupplementaryMaterial>();
        public DbSet<BibliographicReference> BibliographicReferences => Set<BibliographicReference>();

        public DbSet<DiscussionTopic> DiscussionTopics => Set<DiscussionTopic>();
        public DbSet<DiscussionPost> DiscussionPosts => Set<DiscussionPost>();
        public DbSet<AcademicQuestion> AcademicQuestions => Set<AcademicQuestion>();
        public DbSet<AcademicQuestionReply> AcademicQuestionReplies => Set<AcademicQuestionReply>();
        public DbSet<Announcement> Announcements => Set<Announcement>();
        public DbSet<SupportRequest> SupportRequests => Set<SupportRequest>();
        public DbSet<SupportMessage> SupportMessages => Set<SupportMessage>();
        public DbSet<StudentNotification> StudentNotifications => Set<StudentNotification>();

        public DbSet<AcademicLocation> AcademicLocations => Set<AcademicLocation>();
        public DbSet<LocationDirection> LocationDirections => Set<LocationDirection>();

        public DbSet<StudentPersonalFile> StudentPersonalFiles => Set<StudentPersonalFile>();
        public DbSet<PersonalDetailChangeRequest> PersonalDetailChangeRequests => Set<PersonalDetailChangeRequest>();
        public DbSet<PlatformAccessLog> PlatformAccessLogs => Set<PlatformAccessLog>();

        public EfContext(DbContextOptions<EfContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(EfContext))!);
        }
    }

    internal class EfContextFactory : IDesignTimeDbContextFactory<EfContext>
    {
        private const string ApplicationName = "ConnectX.Ef";

        public EfContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfContext>();
            string connectionString = "Host=localhost;Port=5432;Database=ConnectX.Api;Username=postgres;Password=postgres";

            string applicationConnectionString = $"Application Name={ApplicationName};{connectionString}";

            optionsBuilder.UseNpgsql(applicationConnectionString, x =>
            {
                x.EnableRetryOnFailure();
            });

            return new EfContext(optionsBuilder.Options);
        }
    }
}
