using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IUserSubscriptionService : IEntityService<UserSubscription>,
    IUserSubscriptionRepositoryCustom<UserSubscription>
{
}

public interface IUserSubscriptionServiceCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
    Task<TEntity?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true);
}