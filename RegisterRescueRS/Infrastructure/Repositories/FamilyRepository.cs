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
            .Where(x => x.Active)
            .Include(x => x.Houseds)
            .Include(x => x.Shelter)
            .Where(x => shelterId == null || x.ShelterId == shelterId)
            .Where(x => searchTerm == null || x.Houseds.Any(p => p.Active && p.Name.ToUpper().Contains(searchTerm.ToUpper())) || x.Houseds.Any(p => p.Active && !string.IsNullOrEmpty(p.Cellphone) && p.Cellphone.ToUpper().Contains(searchTerm.ToUpper())))
            .OrderByDescending(x => x.RegisteredAt)
            .ApplyPagination(this._pagination, x => lastDate == null || x.RegisteredAt < lastDate)
            .ToListAsync();
    }

    public async Task<FamilyEntity?> GetFamilyById(Guid familyId) =>
        await this._db.Families
            .Include(x => x.Shelter)
            .Include(x => x.Houseds.Where(x => x.Active))
            .FirstOrDefaultAsync(x => x.Active && x.FamilyId == familyId);
}