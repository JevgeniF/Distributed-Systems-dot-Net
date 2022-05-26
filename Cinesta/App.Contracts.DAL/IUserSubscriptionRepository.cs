using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUserSubscriptionRepository : IEntityRepository<UserSubscription>,
    IUserSubscriptionRepositoryCustom<UserSubscription>
{
}

public interface IUserSubscriptionRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
    Task<TEntity?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true);
}