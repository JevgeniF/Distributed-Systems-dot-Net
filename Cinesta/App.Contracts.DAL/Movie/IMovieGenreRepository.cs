using App.Domain.Movie;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Movie;

public interface IMovieGenreRepository : IEntityRepository<MovieGenre>
{
    Task<IEnumerable<MovieGenre>> GetWithInclude(bool noTracking = true);
    IQueryable<MovieGenre> QueryableWithInclude(bool noTracking = true);
}