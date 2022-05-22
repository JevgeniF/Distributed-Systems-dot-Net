using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class UserSubscriptionMapper : BaseMapper<UserSubscription, BLL.DTO.UserSubscription>
{
    public UserSubscriptionMapper(IMapper mapper) : base(mapper)
    {
    }
}