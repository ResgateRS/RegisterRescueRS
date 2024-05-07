using Microsoft.EntityFrameworkCore;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

namespace RegisterRescueRS.Infrastructure.Repositories;

public class ShelterRepository(RegisterRescueRSDbContext dbContext) : IRepository
{
    private readonly RegisterRescueRSDbContext _db = dbContext;

    public async Task<ShelterEntity?> GetShelter(string login, string password) =>
        await this._db.Shelters
            .FirstOrDefaultAsync(e => e.Login == login && e.Password == password);

    public async Task<ShelterEntity> InsertOrUpdate(ShelterEntity user)
    {
        if (user.ShelterId == Guid.Empty)
        {
            user.ShelterId = Guid.NewGuid();
            await this._db.Shelters.AddAsync(user);
        }
        else
            this._db.Shelters.Update(user);

        await this._db.SaveChangesAsync();

        return user;
    }

    public async Task<ShelterEntity?> GetShelterById(Guid shelterId) =>
        await this._db.Shelters
            .FirstOrDefaultAsync(e => e.ShelterId == shelterId);

    public async Task<bool> ShelterExistsById(Guid shelterId) =>
        await this._db.Shelters
            .AnyAsync(e => e.ShelterId == shelterId);
}