using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Pagination;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class HousedRepository(RegisterRescueRSDbContext dbContext, PaginationDTO pagination) : BaseRepository(dbContext, pagination)
{
    public async Task UpsertRange(IEnumerable<HousedEntity> houseds, Guid? familyId = null)
    {
        var entities = await this._db.Houseds
            .Where(x => x.Active && x.FamilyId == (familyId ?? houseds.First().FamilyId))
            .ToListAsync();

        entities.ForEach(x =>
        {
            x.Active = false;
            x.UpdatedAt = DateTimeOffset.Now;
        });

        await _db.Houseds.AddRangeAsync(houseds);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<HousedEntity>> GetHouseds(string? searchTerm)
    {
        string? lastName = await this._db.Houseds
                        .Where(x => x.HousedId == (Guid?)this._pagination.cursor)
                        .Select(x => (string?)x.Name)
                        .FirstOrDefaultAsync();

        return await _db.Houseds
            .Where(x => x.Active && x.Family.Active)
            .Include(x => x.Family)
                .ThenInclude(x => x.Shelter)
            .Where(x => searchTerm == null || x.Name.ToUpper().Contains(searchTerm.ToUpper()) || !string.IsNullOrEmpty(x.Cellphone) && x.Cellphone.ToUpper().Contains(searchTerm.ToUpper()))
            .OrderBy(x => x.Name)
            .ApplyPagination(this._pagination, x => lastName == null || string.Compare(x.Name, lastName) > 0)
            .ToListAsync();
    }
}