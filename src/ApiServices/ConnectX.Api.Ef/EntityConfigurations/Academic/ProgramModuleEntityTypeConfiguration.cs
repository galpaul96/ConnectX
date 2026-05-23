using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Academic
{
    internal class ProgramModuleEntityTypeConfiguration : IEntityTypeConfiguration<ProgramModule>
    {
        public void Configure(EntityTypeBuilder<ProgramModule> configuration)
        {
            configuration.ConfigureAuditedEntity("ProgramModules");

            configuration.Property(o => o.Code).HasMaxLength(64);
            configuration.Property(o => o.Name).HasMaxLength(256);
            configuration.Property(o => o.CreditValue).HasPrecision(6, 2);

            configuration.HasIndex(o => new { o.StudyProgramId, o.SequenceNumber });
            configuration.HasIndex(o => new { o.StudyProgramId, o.Code }).IsUnique();
            configuration.HasIndex(o => o.CoordinatorTeacherId);

            configuration.HasOne(o => o.StudyProgram)
                .WithMany(o => o.Modules)
                .HasForeignKey(o => o.StudyProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration.HasOne(o => o.CoordinatorTeacher)
                .WithMany(o => o.CoordinatedModules)
                .HasForeignKey(o => o.CoordinatorTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
