using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class PaymentDetailsModel : BaseEntityModel<PaymentDetails, BLL.DTO.PaymentDetails, IPaymentDetailsService>,
    IPaymentDetailsModel
{
    public PaymentDetailsModel(IPaymentDetailsService service,
        IMapper<PaymentDetails, BLL.DTO.PaymentDetails> mapper) : base(
        service, mapper)
    {
    }

    public async Task<PaymentDetails?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeGetByUserIdAsync(userId, noTracking));
    }
}