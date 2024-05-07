using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Pagination;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class ShelterNeedsRepository(RegisterRescueRSDbContext dbContext, PaginationDTO pagination) : BaseRepository(dbContext, pagination)
{
    public async Task<ShelterNeedsEntity> InsertOrUpdate(ShelterNeedsEntity entity)
    {
        if (entity.ShelterId == Guid.Empty)
        {
            entity.ShelterNeedsId = Guid.NewGuid();
            await this._db.ShelterNeeds.AddAsync(entity);
        }
        else
            this._db.ShelterNeeds.Update(entity);

        await this._db.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<ShelterNeedsEntity>> ListDonations()
    {
        DateTimeOffset? lastDate = await this._db.ShelterNeeds
                        .Where(x => x.ShelterId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.UpdatedAt)
                        .FirstOrDefaultAsync();

        return await this._db.ShelterNeeds
            .Include(x => x.Shelter)
            .Where(x => x.AcceptingDonations)
            .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ShelterNeedsEntity>> ListVolunteers()
    {
        DateTimeOffset? lastDate = await this._db.ShelterNeeds
                        .Where(x => x.ShelterId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.UpdatedAt)
                        .FirstOrDefaultAsync();

        return await this._db.ShelterNeeds
            .Include(x => x.Shelter)
            .Where(x => x.AcceptingVeterinarians || x.AcceptingDoctors || x.AcceptingVolunteers)
            .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
            .ToListAsync();
    }
}