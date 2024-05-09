using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Pagination;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class ShelterNeedsRepository(RegisterRescueRSDbContext dbContext, PaginationDTO pagination) : BaseRepository(dbContext, pagination)
{
    public async Task<ShelterNeedsEntity> InsertOrUpdate(ShelterNeedsEntity entity)
    {
        entity.ShelterNeedsId = Guid.NewGuid();
        await this._db.ShelterNeeds.AddAsync(entity);

        this._db.RemoveRange(this._db.ShelterNeeds.Where(x => x.ShelterNeedsId != entity.ShelterId && x.ShelterId == entity.ShelterId));

        await this._db.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<ShelterNeedsEntity>> ListDonations(double? latitude, double? longitude)
    {
        DateTimeOffset? lastDate = await this._db.ShelterNeeds
                        .Where(x => x.ShelterId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.UpdatedAt)
                        .FirstOrDefaultAsync();

        if (latitude != null && longitude != null)
            return await this._db.ShelterNeeds
                .Include(x => x.Shelter)
                .Where(x => x.AcceptingDonations)
                .OrderBy(x => x.Shelter.GetDistance(latitude.Value, longitude.Value))
                .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
                .ToListAsync();
        else
            return await this._db.ShelterNeeds
                .Include(x => x.Shelter)
                .Where(x => x.AcceptingDonations)
                .OrderBy(x => x.UpdatedAt)
                .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
                .ToListAsync();
    }

    public async Task<IEnumerable<ShelterNeedsEntity>> ListVolunteers(double? latitude, double? longitude)
    {
        DateTimeOffset? lastDate = await this._db.ShelterNeeds
                        .Where(x => x.ShelterId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.UpdatedAt)
                        .FirstOrDefaultAsync();

        if (latitude != null && longitude != null)
            return await this._db.ShelterNeeds
                .Include(x => x.Shelter)
                .Where(x => x.AcceptingVeterinarians || x.AcceptingDoctors || x.AcceptingVolunteers)
                .OrderBy(x => x.Shelter.GetDistance(latitude.Value, longitude.Value))
                .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
                .ToListAsync();
        else
            return await this._db.ShelterNeeds
                .Include(x => x.Shelter)
                .Where(x => x.AcceptingVeterinarians || x.AcceptingDoctors || x.AcceptingVolunteers)
                .OrderBy(x => x.UpdatedAt)
                .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
                .ToListAsync();
    }
}