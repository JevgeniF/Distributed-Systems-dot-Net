using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class SubscriptionMapper : BaseMapper<Subscription, BLL.DTO.Subscription>
{
    public SubscriptionMapper(IMapper mapper) : base(mapper)
    {
    }
}