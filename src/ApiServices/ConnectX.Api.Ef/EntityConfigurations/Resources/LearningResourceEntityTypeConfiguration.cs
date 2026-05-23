using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Resources
{
    internal class LearningResourceEntityTypeConfiguration : IEntityTypeConfiguration<LearningResource>
    {
        public void Configure(EntityTypeBuilder<LearningResource> configuration)
        {
            configuration.ConfigureAuditedEntity("LearningResources");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.Url).HasMaxLength(1024);
            configuration.Property(o => o.FileName).HasMaxLength(512);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.SortOrder });
            configuration.HasIndex(o => new { o.MeetingId, o.SortOrder });
            configuration.HasIndex(o => new { o.ProgramModuleId, o.ResourceType });
            configuration.HasIndex(o => o.PublishedByTeacherId);
            configuration.HasIndex(o => o.BibliographicReferenceId);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.Resources)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany(o => o.Resources)
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.PublishedByTeacher)
                .WithMany()
                .HasForeignKey(o => o.PublishedByTeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.BibliographicReference)
                .WithMany(o => o.LearningResources)
                .HasForeignKey(o => o.BibliographicReferenceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
