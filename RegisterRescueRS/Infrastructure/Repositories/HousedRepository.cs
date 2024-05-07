using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class HousedRepository(RegisterRescueRSDbContext dbContext) : IRepository
{
    private readonly RegisterRescueRSDbContext _db = dbContext;

    public async Task UpsertRange(IEnumerable<HousedEntity> houseds)
    {
        var query = this._db.Houseds
            .Where(x => x.FamilyId == houseds.First().FamilyId);

        this._db.Houseds.RemoveRange(query);

        houseds.ToList().ForEach(x => x.HousedId = Guid.NewGuid());

        await _db.Houseds.AddRangeAsync(houseds);
        await _db.SaveChangesAsync();
    }
}