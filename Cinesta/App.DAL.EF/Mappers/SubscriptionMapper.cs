using App.DAL.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class SubscriptionMapper : BaseMapper<Subscription, Domain.Subscription>
{
    public SubscriptionMapper(IMapper mapper) : base(mapper)
    {
    }
}