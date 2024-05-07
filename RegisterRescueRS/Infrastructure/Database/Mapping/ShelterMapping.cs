using RegisterRescueRS.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RegisterRescueRS.Infrastructure.Database.Mapping;
public class ShelterMapping : IEntityTypeConfiguration<ShelterEntity>
{
    public void Configure(EntityTypeBuilder<ShelterEntity> builder)
    {
        builder.HasKey(e => e.ShelterId);

        builder.Property(e => e.ShelterId);
        builder.Property(e => e.Login);
        builder.Property(e => e.Password);
        builder.Property(e => e.ShelterName);
        builder.HasMany(e => e.Families)
            .WithOne(x => x.Shelter)
            .HasForeignKey(e => e.ShelterId);
        builder.HasMany(e => e.ShelterNeeds)
            .WithOne(x => x.Shelter)
            .HasForeignKey(e => e.ShelterId);
    }
}