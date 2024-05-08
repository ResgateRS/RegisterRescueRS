using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Pagination;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class FamilyRepository(RegisterRescueRSDbContext dbContext, PaginationDTO pagination) : BaseRepository(dbContext, pagination)
{
    public async Task<FamilyEntity> InsertOrUpdate(FamilyEntity entity)
    {
        if (entity.FamilyId == Guid.Empty)
        {
            entity.FamilyId = Guid.NewGuid();
            await _db.Families.AddAsync(entity);
        }
        else
            _db.Families.Update(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<FamilyEntity>> ListFamilies(string? searchTerm, Guid? shelterId = null)
    {
        DateTimeOffset? lastDate = await this._db.Families
                        .Where(x => x.FamilyId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.RegisteredAt)
                        .FirstOrDefaultAsync();

        return await _db.Families
            .Include(x => x.Houseds)
            .Include(x => x.Shelter)
            .Where(x => shelterId == null || x.ShelterId == shelterId)
            .Where(x => searchTerm == null || x.Houseds.Any(p => p.Name.Contains(searchTerm)) || x.Houseds.Any(p => !string.IsNullOrEmpty(p.Cellphone) && p.Cellphone.Contains(searchTerm)))
            .OrderByDescending(x => x.RegisteredAt)
            .ApplyPagination(this._pagination, x => lastDate == null || x.RegisteredAt < lastDate)
            .ToListAsync();
    }

    public async Task<FamilyEntity?> GetFamilyById(Guid familyId) =>
        await this._db.Families
            .Include(x => x.Houseds)
            .Include(x => x.Shelter)
            .FirstOrDefaultAsync(x => x.FamilyId == familyId);
}