using RegisterRescueRS.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RegisterRescueRS.Infrastructure.Database.Mapping;
public class ShelterNeedsMapping : IEntityTypeConfiguration<ShelterNeedsEntity>
{
    public void Configure(EntityTypeBuilder<ShelterNeedsEntity> builder)
    {
        builder.HasKey(e => e.ShelterNeedsId);

        builder.Property(e => e.ShelterNeedsId);
        builder.Property(e => e.ShelterId);
        builder.Property(e => e.AcceptingVolunteers)
            .HasConversion(v => v ? 1 : 0, v => v == 1);
        builder.Property(e => e.AcceptingDoctors)
            .HasConversion(v => v ? 1 : 0, v => v == 1);
        builder.Property(e => e.AcceptingVeterinarians)
            .HasConversion(v => v ? 1 : 0, v => v == 1);
        builder.Property(e => e.AcceptingDonations)
            .HasConversion(v => v ? 1 : 0, v => v == 1);
        builder.Property(e => e.DonationDescription);
        builder.Property(e => e.VolunteersSubscriptionLink);
        builder.Property(e => e.UpdatedAt);
        builder.HasOne(e => e.Shelter)
            .WithOne(x => x.ShelterNeeds)
            .HasForeignKey<ShelterEntity>(e => e.ShelterId);
    }
}