using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class PaymentDetailsMapper : BaseMapper<PaymentDetails, BLL.DTO.PaymentDetails>
{
    public PaymentDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}