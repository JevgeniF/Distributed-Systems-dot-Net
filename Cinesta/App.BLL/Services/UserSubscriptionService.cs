using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class UserSubscriptionService :
    BaseEntityService<UserSubscription, DAL.DTO.UserSubscription, IUserSubscriptionRepository>, IUserSubscriptionService
{
    public UserSubscriptionService(IUserSubscriptionRepository repository,
        IMapper<UserSubscription, DAL.DTO.UserSubscription> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<UserSubscription>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.IncludeGetAllByUserIdAsync(userId, noTracking)).Select(u => Mapper.Map(u)!);
    }

    public async Task<UserSubscription?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id, noTracking));
    }

    public async Task<UserSubscription?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeGetByUserIdAsync(userId, noTracking));
    }
}