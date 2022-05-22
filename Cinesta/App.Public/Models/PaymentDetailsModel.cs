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

    public async Task<IEnumerable<PaymentDetails>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return (await Service.IncludeGetAllByUserIdAsync(userId, noTracking)).Select(p => Mapper.Map(p)!);
    }
}