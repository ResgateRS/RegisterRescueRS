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

    public async Task<IEnumerable<ShelterNeedsEntity>> ListDonations(double? latitude, double? longitude, string? searchTerm)
    {
        DateTimeOffset? lastDate = await this._db.ShelterNeeds
                        .Where(x => x.ShelterId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.UpdatedAt)
                        .FirstOrDefaultAsync();

        var baseQuery = this._db.ShelterNeeds
            .Include(x => x.Shelter)
            .Where(x => x.Shelter.Login != "adm")
            .Where(x => x.AcceptingDonations)
            .Where(x => searchTerm == null || x.Shelter.ShelterName.Contains(searchTerm) || (x.DonationDescription != null && x.DonationDescription.Contains(searchTerm)));

        if (latitude != null && longitude != null)
            baseQuery = baseQuery
                .OrderBy(x => x.Shelter.GetDistance(latitude.Value, longitude.Value));
        else
            baseQuery = baseQuery
                .OrderBy(x => x.UpdatedAt);

        return await baseQuery
                .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
                .ToListAsync();
    }

    public async Task<IEnumerable<ShelterNeedsEntity>> ListVolunteers(double? latitude, double? longitude, string? searchTerm)
    {
        DateTimeOffset? lastDate = await this._db.ShelterNeeds
                        .Where(x => x.ShelterId == (Guid?)this._pagination.cursor)
                        .Select(x => (DateTimeOffset?)x.UpdatedAt)
                        .FirstOrDefaultAsync();

        var baseQuery = this._db.ShelterNeeds
            .Include(x => x.Shelter)
            .Where(x => x.Shelter.Login != "adm")
            .Where(x => x.AcceptingVeterinarians || x.AcceptingDoctors || x.AcceptingVolunteers || x.Avaliable)
            .Where(x => searchTerm == null || x.Shelter.ShelterName.Contains(searchTerm));

        if (latitude != null && longitude != null)
            baseQuery = baseQuery
                .OrderBy(x => x.Shelter.GetDistance(latitude.Value, longitude.Value));
        else
            baseQuery = baseQuery
                .OrderBy(x => x.UpdatedAt);

        return await baseQuery
            .ApplyPagination(this._pagination, x => lastDate == null || x.UpdatedAt < lastDate)
            .ToListAsync();
    }

    public async Task<ShelterNeedsEntity?> GetShelterNeeds(Guid shelterId) =>
        await this._db.ShelterNeeds
            .FirstOrDefaultAsync(x => x.ShelterId == shelterId);
}