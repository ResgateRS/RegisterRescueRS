using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class HousedRepository(RegisterRescueRSDbContext dbContext) : IRepository
{
    private readonly RegisterRescueRSDbContext _db = dbContext;

    internal async Task InsertRange(IEnumerable<HousedEntity> houseds)
    {
        await _db.Houseds.AddRangeAsync(houseds);
        await _db.SaveChangesAsync();
    }
}