using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMovieGenreRepository : IEntityRepository<MovieGenre>
{
    Task<IEnumerable<DTO.MovieGenre>> IncludeGetAllAsync(bool noTracking = true);
    Task<DTO.MovieGenre?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}