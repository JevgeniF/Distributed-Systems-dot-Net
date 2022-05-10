using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface ISubscriptionRepository : IEntityRepository<Subscription>
{
    //Task<IEnumerable<Subscription>> GetAllByUserIdAsync(Guid userId, bool noTracking = true);
}