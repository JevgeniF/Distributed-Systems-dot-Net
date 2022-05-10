using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class SubscriptionService: BaseEntityService<Subscription, DAL.DTO.Subscription, ISubscriptionRepository>, ISubscriptionService
{
    public SubscriptionService(ISubscriptionRepository repository, IMapper<Subscription, DAL.DTO.Subscription> mapper) : base(repository, mapper)
    {
    }
}