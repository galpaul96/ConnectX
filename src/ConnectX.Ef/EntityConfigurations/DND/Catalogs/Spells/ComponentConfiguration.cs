using ConnectX.Domain.Entities.DND.Catalogs.Spells;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Ef.EntityConfigurations.DND.Catalogs.Spells;

internal sealed class ComponentConfiguration : IEntityTypeConfiguration<SpellComponent>
{
    public void Configure(EntityTypeBuilder<SpellComponent> builder)
    {
        builder.ConfigureAuditedEntity("SpellComponents");
        builder.Property(x => x.Type).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Label).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Position).IsRequired();
        builder.Property(x => x.Value).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.Html).HasColumnType("text").IsRequired();
        builder.HasIndex(x => new { x.SpellId, x.Type }).IsUnique();
        builder.HasIndex(x => new { x.SpellId, x.Position });
    }
}
