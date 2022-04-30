using App.Domain.User;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.User;

public interface IPaymentDetailsRepository : IEntityRepository<PaymentDetails>
{
    Task<IEnumerable<PaymentDetails>> GetAllByUserIdAsync(Guid userId, bool noTracking = true);
}