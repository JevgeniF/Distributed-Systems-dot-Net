using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PaymentDetailsMapper : BaseMapper<PaymentDetails, Domain.PaymentDetails>
{
    public PaymentDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}