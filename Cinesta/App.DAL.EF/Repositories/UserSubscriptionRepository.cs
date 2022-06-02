using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UserSubscriptionRepository : BaseEntityRepository<UserSubscription, Domain.UserSubscription, AppDbContext>,
    IUserSubscriptionRepository
{
    public UserSubscriptionRepository(AppDbContext dbContext,
        IMapper<UserSubscription, Domain.UserSubscription> mapper) :
        base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<UserSubscription>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.Subscription)
            .Include(u => u.AppUser).Where(u => u.AppUserId == userId);

        return (await query.ToListAsync()).Select(u => Mapper.Map(u)!);
    }

    public async Task<UserSubscription?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.Subscription)
            .Include(u => u.AppUser);

        return Mapper.Map(await query.FirstOrDefaultAsync(u => u.Id == id));
    }

    public async Task<UserSubscription?> IncludeGetByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.Subscription)
            .Include(u => u.AppUser);

        return Mapper.Map(await query.FirstOrDefaultAsync(u => u.AppUserId == userId));
    }
}