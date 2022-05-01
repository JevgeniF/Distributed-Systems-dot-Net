using App.Domain.Movie;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Movie;

public interface IMovieDBScoreRepository : IEntityRepository<MovieDbScore>
{
    Task<IEnumerable<MovieDbScore>> GetWithInclude(bool noTracking = true);
    IQueryable<MovieDbScore> QueryableWithInclude(bool noTracking = true);
}