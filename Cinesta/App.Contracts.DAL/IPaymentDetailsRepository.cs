using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPaymentDetailsRepository : IEntityRepository<PaymentDetails>,
    IPaymentDetailsRepositoryCustom<PaymentDetails>
{
}

public interface IPaymentDetailsRepositoryCustom<TEntity>
{
    Task<TEntity?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true);
}