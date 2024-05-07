using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Pagination;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class ShelterNeedsRepository(RegisterRescueRSDbContext dbContext, PaginationDTO pagination) : BaseRepository(dbContext, pagination)
{
    public async Task<ShelterNeedEntity> InsertOrUpdate(ShelterNeedEntity entity)
    {
        if (entity.ShelterId == Guid.Empty)
        {
            entity.ShelterNeedId = Guid.NewGuid();
            await this._db.ShelterNeeds.AddAsync(entity);
        }
        else
            this._db.ShelterNeeds.Update(entity);

        await this._db.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<ShelterNeedEntity>> ListNeeds(bool? acceptingVolunteers)
    {
        DateTimeOffset? lastDate = await this._db.ShelterNeeds
                        .Where(x => x.ShelterId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.UpdatedAt)
                        .FirstOrDefaultAsync();

        return await this._db.ShelterNeeds
            .Where(x => acceptingVolunteers == null || x.AcceptingVolunteers == acceptingVolunteers)
            .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
            .ToListAsync();
    }
}