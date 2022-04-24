using App.Contracts.DAL.User;
using App.Domain.User;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.User;

public class SubscriptionRepository : BaseEntityRepository<Subscription, AppDbContext>, ISubscriptionRepository
{
    public SubscriptionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}