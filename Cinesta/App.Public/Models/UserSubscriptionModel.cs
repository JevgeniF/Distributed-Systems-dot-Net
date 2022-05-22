using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class UserSubscriptionModel :
    BaseEntityModel<UserSubscription, BLL.DTO.UserSubscription, IUserSubscriptionService>,
    IUserSubscriptionModel
{
    public UserSubscriptionModel(IUserSubscriptionService service,
        IMapper<UserSubscription, BLL.DTO.UserSubscription> mapper) : base(
        service, mapper)
    {
    }
}