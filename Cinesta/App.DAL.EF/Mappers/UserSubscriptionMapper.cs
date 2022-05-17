using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class UserSubscriptionMapper : BaseMapper<UserSubscription, Domain.UserSubscription>
{
    public UserSubscriptionMapper(IMapper mapper) : base(mapper)
    {
    }
}