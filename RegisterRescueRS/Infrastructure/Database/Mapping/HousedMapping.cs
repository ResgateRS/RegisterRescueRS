using RegisterRescueRS.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RegisterRescueRS.Infrastructure.Database.Mapping;
public class HousedMapping : IEntityTypeConfiguration<HousedEntity>
{
    public void Configure(EntityTypeBuilder<HousedEntity> builder)
    {
        builder.HasKey(e => e.HousedId);

        builder.Property(e => e.HousedId);
        builder.Property(e => e.FamilyId);
        builder.HasOne(e => e.Family)
            .WithMany(x => x.Houseds)
            .HasForeignKey(e => e.FamilyId);
        builder.Property(e => e.Name);
        builder.Property(e => e.Age);
        builder.Property(e => e.Cellphone);
        builder.Property(e => e.IsFamilyResponsable)
            .HasConversion(v => v ? 1 : 0, v => v == 1);
        builder.Property(e => e.RegisteredAt);
        builder.Property(e => e.UpdatedAt);
    }
}