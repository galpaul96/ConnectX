using ConnectX.Domain.Entities.DND.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Ef.EntityConfigurations.DND.Catalogs.Items;

internal sealed class ComponentConfiguration : IEntityTypeConfiguration<ItemComponent>
{
    public void Configure(EntityTypeBuilder<ItemComponent> builder)
    {
        builder.ConfigureAuditedEntity("ItemComponents");
        builder.Property(x => x.Type).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Label).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Position).IsRequired();
        builder.Property(x => x.Value).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.Html).HasColumnType("text").IsRequired();
        builder.HasIndex(x => new { x.ItemId, x.Type }).IsUnique();
        builder.HasIndex(x => new { x.ItemId, x.Position });
    }
}
