using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database.Mapping;

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FamilyMapping());
            modelBuilder.ApplyConfiguration(new ShelterMapping());
            modelBuilder.ApplyConfiguration(new ShelterNeedsMapping());
            modelBuilder.ApplyConfiguration(new HousedMapping());
        }
    }
}