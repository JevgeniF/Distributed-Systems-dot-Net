using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IUserRatingService : IEntityService<UserRating>, IUserRatingRepositoryCustom<UserRating>
{
}

public interface IUserRatingServiceCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}