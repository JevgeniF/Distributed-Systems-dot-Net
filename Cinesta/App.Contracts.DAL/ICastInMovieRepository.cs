using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface ICastInMovieRepository : IEntityRepository<CastInMovie>, ICastInMovieRepositoryCustom<CastInMovie>
{
}

public interface ICastInMovieRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetByMovie(Guid movieId, bool noTracking = true);
}