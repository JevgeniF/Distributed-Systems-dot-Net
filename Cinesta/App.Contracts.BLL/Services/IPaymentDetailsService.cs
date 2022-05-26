using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IPaymentDetailsService : IEntityService<PaymentDetails>,
    IPaymentDetailsRepositoryCustom<PaymentDetails>
{
}

public interface IPaymentDetailsServiceCustom<TEntity>
{
    Task<TEntity?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true);
}