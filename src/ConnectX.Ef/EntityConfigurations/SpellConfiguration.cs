using ConnectX.Domain.Entities.Catalogs.Spells;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectX.Ef.EntityConfigurations;

internal sealed class SpellConfiguration : IEntityTypeConfiguration<Spell>
{
    public void Configure(EntityTypeBuilder<Spell> builder)
    {
        builder.ConfigureAuditedEntity("Spells");

        builder.Property(spell => spell.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasMany(spell => spell.Components)
            .WithOne(x => x.Spell)
            .HasForeignKey(x => x.SpellId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
