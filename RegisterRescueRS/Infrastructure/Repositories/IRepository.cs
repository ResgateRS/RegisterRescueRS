using RegisterRescueRS.Infrastructure.Database;
using RegisterRescueRS.Pagination;
namespace RegisterRescueRS.Infrastructure.Repositories;


public interface IRepository { }

public abstract class BaseRepository
{

    protected readonly RegisterRescueRSDbContext _db;

    protected readonly PaginationDTO _pagination;

    public BaseRepository(RegisterRescueRSDbContext _dbContext, PaginationDTO? pagination = null) =>
        (_db, _pagination) = (_dbContext, pagination ?? new PaginationDTO());
}