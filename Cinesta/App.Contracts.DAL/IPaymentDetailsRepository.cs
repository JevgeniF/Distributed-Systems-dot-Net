using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPaymentDetailsRepository : IEntityRepository<PaymentDetails>
{
    Task<IEnumerable<PaymentDetails>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true);
}