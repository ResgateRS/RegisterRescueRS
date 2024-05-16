using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Infrastructure.Database
{
    public class RegisterRescueRSDbContext(DbContextOptions<RegisterRescueRSDbContext> options) : DbContext(options)
    {
        public DbSet<FamilyEntity> Families => Set<FamilyEntity>();
        public DbSet<ShelterEntity> Shelters => Set<ShelterEntity>();
        public DbSet<ShelterNeedsEntity> ShelterNeeds => Set<ShelterNeedsEntity>();
        public DbSet<HousedEntity> Houseds => Set<HousedEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FamilyEntity>()
                .HasKey(f => f.FamilyId);

            modelBuilder.Entity<ShelterEntity>()
                .HasKey(f => f.ShelterId);

            modelBuilder.Entity<HousedEntity>()
                .HasKey(f => f.HousedId);

            modelBuilder.Entity<ShelterNeedsEntity>()
                .HasKey(f => f.ShelterNeedsId);

            modelBuilder.Entity<ShelterNeedsEntity>()
                .Property(e => e.AcceptingVolunteers)
                .HasConversion(v => v ? 1 : 0, v => v == 1);
            modelBuilder.Entity<ShelterNeedsEntity>()
                .Property(e => e.AcceptingDoctors)
                .HasConversion(v => v ? 1 : 0, v => v == 1);
            modelBuilder.Entity<ShelterNeedsEntity>()
                .Property(e => e.AcceptingVeterinarians)
                .HasConversion(v => v ? 1 : 0, v => v == 1);
            modelBuilder.Entity<ShelterNeedsEntity>()
                .Property(e => e.AcceptingDonations)
                .HasConversion(v => v ? 1 : 0, v => v == 1);
            modelBuilder.Entity<ShelterNeedsEntity>()
                .Property(e => e.Avaliable)
                .HasConversion(v => v ? 1 : 0, v => v == 1);

            modelBuilder.Entity<HousedEntity>()
                .Property(e => e.IsFamilyResponsable)
                .HasConversion(v => v ? 1 : 0, v => v == 1);
            modelBuilder.Entity<HousedEntity>()
                .Property(e => e.Active)
                .HasConversion(v => v ? 1 : 0, v => v == 1)
                .HasDefaultValue(true);

            modelBuilder.Entity<FamilyEntity>()
                .Property(e => e.Active)
                .HasConversion(v => v ? 1 : 0, v => v == 1)
                .HasDefaultValue(true);

            modelBuilder.Entity<ShelterEntity>()
                .Property(e => e.Adm)
                .HasDefaultValue(false)
                .HasConversion(v => v ? 1 : 0, v => v == 1);
            modelBuilder.Entity<ShelterEntity>()
                .Property(e => e.Verified)
                .HasDefaultValue(false)
                .HasConversion(v => v != null && v.Value ? 1 : 0, v => v == 1);
        }

    }
}