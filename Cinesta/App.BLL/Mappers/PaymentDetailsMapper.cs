using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PaymentDetailsMapper : BaseMapper<PaymentDetails, DAL.DTO.PaymentDetails>
{
    public PaymentDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}