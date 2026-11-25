using ConnectX.Domain.Entities.Catalogs.Spells;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Ef.EntityConfigurations;

internal sealed class ComponentConfiguration : IEntityTypeConfiguration<SpellComponent>
{
    public void Configure(EntityTypeBuilder<SpellComponent> builder)
    {
        builder.ConfigureAuditedEntity("SpellComponents");

        builder.Property<Guid>("SpellId")
            .IsRequired();

        builder.Property(component => component.Label)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(component => component.Key)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(component => component.Value)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(component => component.Html)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex("SpellId", nameof(SpellComponent.Key))
            .IsUnique();
    }
}
