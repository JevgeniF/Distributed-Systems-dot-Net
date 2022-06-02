using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IUnitOfWork
{
    ISubscriptionRepository Subscription { get; }
}