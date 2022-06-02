using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMovieDbScoreService : IEntityService<MovieDbScore>, IMovieDbScoreRepositoryCustom<MovieDbScore>
{
}

public interface IMovieDbScoreServiceCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);

    Task<TEntity?> GetMovieDbScoresForMovie(Guid movieId, bool noTracking = true);
}