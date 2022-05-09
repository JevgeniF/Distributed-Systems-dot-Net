using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMovieDbScoreRepository : IEntityRepository<MovieDbScore>
{
    Task<IEnumerable<MovieDbScore>> IncludeGetAllAsync(bool noTracking = true);
    Task<DTO.MovieDbScore?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}