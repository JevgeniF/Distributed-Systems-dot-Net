using App.Domain.Identity;
using App.Domain.User;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.User;

public interface ISubscriptionRepository: IEntityRepository<Subscription>
{
}