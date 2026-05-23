using ConnectX.Api.Ef.EntityConfigurations;
using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Api.Ef.EntityConfigurations.Communication
{
    internal class SupportMessageEntityTypeConfiguration : IEntityTypeConfiguration<SupportMessage>
    {
        public void Configure(EntityTypeBuilder<SupportMessage> configuration)
        {
            configuration.ConfigureAuditedEntity("SupportMessages");

            configuration.HasIndex(o => new { o.SupportRequestId, o.SentAt });
            configuration.HasIndex(o => new { o.AuthorId, o.AuthorRole });

            configuration.HasOne(o => o.SupportRequest)
                .WithMany(o => o.Messages)
                .HasForeignKey(o => o.SupportRequestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
