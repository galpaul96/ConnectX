using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Learning
{
    internal class LessonContentEntityTypeConfiguration : IEntityTypeConfiguration<LessonContent>
    {
        public void Configure(EntityTypeBuilder<LessonContent> configuration)
        {
            configuration.ConfigureAuditedEntity("LessonContents");

            configuration.Property(o => o.Title).HasMaxLength(256);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.SortOrder });
            configuration.HasIndex(o => new { o.MeetingId, o.SortOrder });
            configuration.HasIndex(o => o.PublishedByTeacherId);
            configuration.HasIndex(o => o.AvailableFrom);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.LessonContents)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany(o => o.LessonContents)
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.PublishedByTeacher)
                .WithMany()
                .HasForeignKey(o => o.PublishedByTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
