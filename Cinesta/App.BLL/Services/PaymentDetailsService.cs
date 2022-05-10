using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class PaymentDetailsService: BaseEntityService<PaymentDetails, DAL.DTO.PaymentDetails, IPaymentDetailsRepository>, IPaymentDetailsService
{
    public PaymentDetailsService(IPaymentDetailsRepository repository, IMapper<PaymentDetails, DAL.DTO.PaymentDetails> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<PaymentDetails>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.IncludeGetAllByUserIdAsync(userId, noTracking)).Select(p => Mapper.Map(p)!);
    }
}