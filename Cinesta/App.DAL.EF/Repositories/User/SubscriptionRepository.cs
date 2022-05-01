using App.Contracts.DAL.User;
using App.Domain.User;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.User;

public class SubscriptionRepository : BaseEntityRepository<Subscription, AppDbContext>, ISubscriptionRepository
{
    public SubscriptionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Subscription>> GetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser).Where(u => u.AppUserId == userId);

        return await query.ToListAsync();
    }
}