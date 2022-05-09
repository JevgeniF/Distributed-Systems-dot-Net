using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUserSubscriptionRepository : IEntityRepository<UserSubscription>
{
    Task<IEnumerable<UserSubscription>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true);
    Task<DTO.UserSubscription?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}