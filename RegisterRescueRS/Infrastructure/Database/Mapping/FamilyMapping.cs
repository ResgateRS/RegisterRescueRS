using RegisterRescueRS.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RegisterRescueRS.Infrastructure.Database.Mapping;
public class FamilyMapping : IEntityTypeConfiguration<FamilyEntity>
{
    public void Configure(EntityTypeBuilder<FamilyEntity> builder)
    {
        builder.HasKey(e => e.FamilyId);

        builder.Property(e => e.FamilyId);
        builder.Property(e => e.ShelterId);
        builder.HasOne(e => e.Shelter)
            .WithMany(x => x.Families)
            .HasForeignKey(e => e.ShelterId);
        builder.Property(e => e.ResponsableId);
        builder.HasOne(e => e.Responsable)
            .WithOne(x => x.FamilyResponsable)
            .HasForeignKey<FamilyEntity>(e => e.ResponsableId);
        builder.Property(e => e.RegisteredAt);
        builder.Property(e => e.UpdatedAt);
    }
}