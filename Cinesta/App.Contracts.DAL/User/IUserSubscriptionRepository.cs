using App.Domain.User;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.User;

public interface IUserSubscriptionRepository: IEntityRepository<UserSubscription>
{
    Task<List<UserSubscription>> GetAllByUserIdAsync(Guid userId, bool noTracking = true);
    IQueryable<UserSubscription> QueryableWithInclude(bool noTracking = true);
}