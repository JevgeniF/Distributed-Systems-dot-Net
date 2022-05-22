using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IProfileFavoriteMovieService : IEntityService<ProfileFavoriteMovie>,
    IProfileFavoriteMovieRepositoryCustom<ProfileFavoriteMovie>
{
}

public interface IProfileFavoriteMovieServiceCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllByProfileIdAsync(Guid profileId,
        bool noTracking = true);

    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}