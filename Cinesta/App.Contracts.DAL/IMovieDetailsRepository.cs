using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMovieDetailsRepository : IEntityRepository<MovieDetails>
{
    Task<IEnumerable<DTO.MovieDetails>> IncludeGetAllAsync(bool noTracking = true);
    Task<DTO.MovieDetails?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
    Task<IEnumerable<DTO.MovieDetails>> IncludeGetByAgeAsync(int age, bool noTracking = true);
}