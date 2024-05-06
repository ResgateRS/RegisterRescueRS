using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class FamilyRepository(RegisterRescueRSDbContext dbContext) : IRepository
{
    private readonly RegisterRescueRSDbContext _db = dbContext;

    public async Task<FamilyEntity> InsertOrUpdate(FamilyEntity entity)
    {
        if (entity.FamilyId == Guid.Empty)
            await _db.Families.AddAsync(entity);
        else
            _db.Families.Update(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<FamilyEntity>> ListFamilies(int page, int size, string searchTerm, Guid shelterId) =>
        await _db.Houseds
            .Include(x => x.Family)
            .Where(x => x.Family.ShelterId == shelterId)
            .Where(x => x.Name.Contains(searchTerm) || x.Cellphone.Contains(searchTerm))
            .Select(x => x.Family)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

    public async Task<FamilyEntity?> GetFamilyById(Guid familyId) =>
        await this._db.Families
            .Include(x => x.Houseds)
            .Include(x => x.Shelter)
            .FirstOrDefaultAsync(x => x.FamilyId == familyId);
}