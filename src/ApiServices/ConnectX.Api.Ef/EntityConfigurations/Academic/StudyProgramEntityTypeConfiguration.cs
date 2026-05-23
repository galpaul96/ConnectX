using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Academic
{
    internal class StudyProgramEntityTypeConfiguration : IEntityTypeConfiguration<StudyProgram>
    {
        public void Configure(EntityTypeBuilder<StudyProgram> configuration)
        {
            configuration.ConfigureAuditedEntity("StudyPrograms");

            configuration.Property(o => o.Code).HasMaxLength(64);
            configuration.Property(o => o.Name).HasMaxLength(256);
            configuration.Property(o => o.Language).HasMaxLength(32);
            configuration.Property(o => o.CreditValue).HasPrecision(6, 2);

            configuration.HasIndex(o => o.Code).IsUnique();
            configuration.HasIndex(o => new { o.IsActive, o.Level });
        }
    }
}
