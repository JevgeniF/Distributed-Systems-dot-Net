using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IProfileFavoriteMovieRepository : IEntityRepository<ProfileFavoriteMovie>,
    IProfileFavoriteMovieRepositoryCustom<ProfileFavoriteMovie>
{
}

public interface IProfileFavoriteMovieRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllByProfileIdAsync(Guid profileId,
        bool noTracking = true);

    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}