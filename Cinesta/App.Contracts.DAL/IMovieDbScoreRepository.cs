using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMovieDbScoreRepository : IEntityRepository<MovieDbScore>, IMovieDbScoreRepositoryCustom<MovieDbScore>
{
}

public interface IMovieDbScoreRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}