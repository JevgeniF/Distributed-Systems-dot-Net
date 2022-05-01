using App.Domain.User;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.User;

public interface ISubscriptionRepository : IEntityRepository<Subscription>
{
    Task<IEnumerable<Subscription>> GetAllByUserIdAsync(Guid userId, bool noTracking = true);
}