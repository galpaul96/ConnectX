using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class AcademicQuestionReplyEntityTypeConfiguration : IEntityTypeConfiguration<AcademicQuestionReply>
    {
        public void Configure(EntityTypeBuilder<AcademicQuestionReply> configuration)
        {
            configuration.ConfigureAuditedEntity("AcademicQuestionReplies");

            configuration.HasIndex(o => new { o.AcademicQuestionId, o.PostedAt });
            configuration.HasIndex(o => new { o.AuthorId, o.AuthorRole });

            configuration.HasOne(o => o.AcademicQuestion)
                .WithMany(o => o.Replies)
                .HasForeignKey(o => o.AcademicQuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
