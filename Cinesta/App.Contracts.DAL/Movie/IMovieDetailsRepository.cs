using App.Domain.Movie;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Movie;

public interface IMovieDetailsRepository : IEntityRepository<MovieDetails>
{
    Task<IEnumerable<MovieDetails>> GetWithInclude(bool noTracking = true);
    Task<IEnumerable<MovieDetails>> GetByAgeRating(int age, bool noTracking = true);
    IQueryable<MovieDetails> QueryableWithInclude(bool noTracking = true);
}