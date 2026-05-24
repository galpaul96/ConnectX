using ConnectX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Web.Data;

internal sealed class UserNotificationEntityTypeConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> configuration)
    {
        configuration.ToTable("UserNotifications");

        configuration.HasKey(notification => notification.Id);

        configuration.Property(notification => notification.Id)
            .ValueGeneratedNever();

        configuration.Property(notification => notification.ExternalId)
            .ValueGeneratedNever();

        configuration.Property(notification => notification.RecipientUserId)
            .HasMaxLength(450);

        configuration.Property(notification => notification.IssuerUserId)
            .HasMaxLength(450);

        configuration.Property(notification => notification.Title)
            .HasMaxLength(256);

        configuration.Property(notification => notification.Body)
            .IsRequired();

        configuration.Property(notification => notification.Topic)
            .HasMaxLength(128);

        configuration.Property(notification => notification.UserViewed)
            .HasDefaultValue(false);

        configuration.Property(notification => notification.ActionUrl)
            .HasMaxLength(1024);

        configuration.Property(notification => notification.SourceType)
            .HasMaxLength(128);

        configuration.Property(notification => notification.SourceId)
            .HasMaxLength(128);

        configuration.HasIndex(notification => notification.ExternalId)
            .IsUnique();

        configuration.HasIndex(notification => new { notification.RecipientUserId, notification.CreatedAt });
        configuration.HasIndex(notification => new { notification.RecipientUserId, notification.UserViewed, notification.CreatedAt });
        configuration.HasIndex(notification => new { notification.RecipientUserId, notification.EventStartsAt });
        configuration.HasIndex(notification => new { notification.RecipientUserId, notification.SourceType, notification.SourceId });
        configuration.HasIndex(notification => new { notification.IsDeleted, notification.UpdatedDate });

        configuration.HasQueryFilter(notification => !notification.IsDeleted);

        configuration.HasOne<ApplicationUser>()
            .WithMany(user => user.Notifications)
            .HasForeignKey(notification => notification.RecipientUserId)
            .OnDelete(DeleteBehavior.Cascade);

        configuration.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(notification => notification.IssuerUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
