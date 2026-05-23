using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Students
{
    internal class PersonalDetailChangeRequestEntityTypeConfiguration : IEntityTypeConfiguration<PersonalDetailChangeRequest>
    {
        public void Configure(EntityTypeBuilder<PersonalDetailChangeRequest> configuration)
        {
            configuration.ConfigureAuditedEntity("PersonalDetailChangeRequests");

            configuration.Property(o => o.FieldName).HasMaxLength(128);

            configuration.HasIndex(o => new { o.StudentId, o.Status });
            configuration.HasIndex(o => new { o.StudentPersonalFileId, o.SubmittedAt });
            configuration.HasIndex(o => o.ReviewedByStaffId);

            configuration.HasOne(o => o.StudentPersonalFile)
                .WithMany(o => o.ChangeRequests)
                .HasForeignKey(o => o.StudentPersonalFileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
