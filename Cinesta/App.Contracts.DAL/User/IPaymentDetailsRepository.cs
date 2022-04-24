using App.Domain.User;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.User;

public interface IPaymentDetailsRepository : IEntityRepository<PaymentDetails>
{
    // custom methods here
}