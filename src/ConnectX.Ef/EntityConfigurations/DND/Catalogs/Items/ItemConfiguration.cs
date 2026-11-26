using ConnectX.Domain.Entities.DND.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Ef.EntityConfigurations.DND.Catalogs.Items;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ConfigureAuditedEntity("Items");
        builder.Property(x => x.SourceId).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
        builder.Property(x => x.StorageFile).HasMaxLength(512).IsRequired();
        builder.HasIndex(x => x.SourceId).IsUnique();
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => new { x.Name, x.Id });
        builder.HasMany(x => x.Components).WithOne(x => x.Item)
            .HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Cascade);
    }
}
