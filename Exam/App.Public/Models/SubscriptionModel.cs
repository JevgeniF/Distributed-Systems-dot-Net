using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class SubscriptionModel : BaseEntityModel<Subscription, BLL.DTO.Subscription, ISubscriptionService>,
    ISubscriptionModel
{
    public SubscriptionModel(ISubscriptionService service, IMapper<Subscription, BLL.DTO.Subscription> mapper) : base(
        service, mapper)
    {
    }
}