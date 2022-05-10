using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UserSubscriptionMapper : BaseMapper<UserSubscription, DAL.DTO.UserSubscription>
{
    public UserSubscriptionMapper(IMapper mapper) : base(mapper)
    {
    }
}