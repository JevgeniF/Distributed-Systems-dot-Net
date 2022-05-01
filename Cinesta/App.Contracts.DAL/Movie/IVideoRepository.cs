using App.Domain.Movie;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Movie;

public interface IVideoRepository : IEntityRepository<Video>
{
    Task<IEnumerable<Video>> GetWithInclude (bool noTracking = true);
    IQueryable<Video> QueryableWithInclude(bool noTracking = true);
}