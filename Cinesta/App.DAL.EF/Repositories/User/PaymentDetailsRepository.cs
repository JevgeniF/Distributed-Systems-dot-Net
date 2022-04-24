using App.Contracts.DAL.User;
using App.Domain.User;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.User;

public class PaymentDetailsRepository: BaseEntityRepository<PaymentDetails, AppDbContext>, IPaymentDetailsRepository
{
    public PaymentDetailsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}