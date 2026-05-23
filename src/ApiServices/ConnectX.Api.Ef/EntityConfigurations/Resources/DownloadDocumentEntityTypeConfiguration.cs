using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Resources
{
    internal class DownloadDocumentEntityTypeConfiguration : IEntityTypeConfiguration<DownloadDocument>
    {
        public void Configure(EntityTypeBuilder<DownloadDocument> configuration)
        {
            configuration.ConfigureAuditedEntity("DownloadDocuments");

            configuration.Property(o => o.Title).HasMaxLength(256);
            configuration.Property(o => o.FileName).HasMaxLength(512);
            configuration.Property(o => o.FileUrl).HasMaxLength(1024);
            configuration.Property(o => o.Version).HasMaxLength(64);

            configuration.HasIndex(o => new { o.StudyProgramId, o.DocumentType });
            configuration.HasIndex(o => new { o.ProgramModuleId, o.DocumentType });
            configuration.HasIndex(o => o.PublishedByTeacherId);
            configuration.HasIndex(o => o.ExpiresAt);

            configuration.HasOne(o => o.StudyProgram)
                .WithMany(o => o.Documents)
                .HasForeignKey(o => o.StudyProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.Documents)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.PublishedByTeacher)
                .WithMany()
                .HasForeignKey(o => o.PublishedByTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
