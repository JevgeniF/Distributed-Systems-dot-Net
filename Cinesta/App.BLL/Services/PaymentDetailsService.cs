using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class PaymentDetailsService :
    BaseEntityService<PaymentDetails, DAL.DTO.PaymentDetails, IPaymentDetailsRepository>, IPaymentDetailsService
{
    public PaymentDetailsService(IPaymentDetailsRepository repository,
        IMapper<PaymentDetails, DAL.DTO.PaymentDetails> mapper) : base(repository, mapper)
    {
    }

    public async Task<PaymentDetails?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeGetByUserIdAsync(userId, noTracking));
    }
}