using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class SubscriptionMapper : BaseMapper<Subscription, DAL.DTO.Subscription>
{
    public SubscriptionMapper(IMapper mapper) : base(mapper)
    {
    }
}