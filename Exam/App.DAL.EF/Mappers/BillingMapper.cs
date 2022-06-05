using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BillingMapper: BaseMapper<Billing, Domain.Billing>
{
    public BillingMapper(IMapper mapper) : base(mapper)
    {
    }
}