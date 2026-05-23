using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Resources
{
    internal class BibliographicReferenceEntityTypeConfiguration : IEntityTypeConfiguration<BibliographicReference>
    {
        public void Configure(EntityTypeBuilder<BibliographicReference> configuration)
        {
            configuration.ConfigureAuditedEntity("BibliographicReferences");

            configuration.Property(o => o.Title).HasMaxLength(512);
            configuration.Property(o => o.Authors).HasMaxLength(1024);
            configuration.Property(o => o.Editor).HasMaxLength(512);
            configuration.Property(o => o.Publisher).HasMaxLength(256);
            configuration.Property(o => o.JournalName).HasMaxLength(256);
            configuration.Property(o => o.Edition).HasMaxLength(64);
            configuration.Property(o => o.Volume).HasMaxLength(64);
            configuration.Property(o => o.Issue).HasMaxLength(64);
            configuration.Property(o => o.PageRange).HasMaxLength(64);
            configuration.Property(o => o.Isbn).HasMaxLength(32);
            configuration.Property(o => o.Issn).HasMaxLength(32);
            configuration.Property(o => o.Doi).HasMaxLength(256);
            configuration.Property(o => o.Url).HasMaxLength(1024);

            configuration.HasIndex(o => new { o.ProgramModuleId, o.SortOrder });
            configuration.HasIndex(o => new { o.ProgramModuleId, o.ReferenceType });
            configuration.HasIndex(o => o.MeetingId);
            configuration.HasIndex(o => o.Doi);
            configuration.HasIndex(o => o.Isbn);

            configuration.HasOne(o => o.ProgramModule)
                .WithMany(o => o.BibliographicReferences)
                .HasForeignKey(o => o.ProgramModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.Meeting)
                .WithMany()
                .HasForeignKey(o => o.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
