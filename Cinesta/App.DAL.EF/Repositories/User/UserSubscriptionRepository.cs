using App.Contracts.DAL.User;
using App.Domain.User;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.User;

public class UserSubscriptionRepository : BaseEntityRepository<UserSubscription, AppDbContext>,
    IUserSubscriptionRepository
{
    public UserSubscriptionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<UserSubscription>> GetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking).Where(u => u.AppUserId == userId);
        query = query.Include(u => u.Subscription).Include(u => u.AppUser);

        return query.ToListAsync();
    }
    
    public IQueryable<UserSubscription> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(u => u.AppUser)
            .Include(u => u.Subscription);
    }
}