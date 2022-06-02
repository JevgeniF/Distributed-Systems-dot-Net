using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMovieGenreRepository : IEntityRepository<MovieGenre>, IMovieGenreRepositoryCustom<MovieGenre>
{
}

public interface IMovieGenreRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}