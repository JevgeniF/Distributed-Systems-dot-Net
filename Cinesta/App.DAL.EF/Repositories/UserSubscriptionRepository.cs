using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UserSubscriptionRepository : BaseEntityRepository<DTO.UserSubscription, UserSubscription, AppDbContext>,
    IUserSubscriptionRepository
{
    public UserSubscriptionRepository(AppDbContext dbContext, IMapper<DTO.UserSubscription, UserSubscription> mapper) :
        base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<DTO.UserSubscription>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.Subscription)
            .Include(u => u.AppUser).Where(u => u.AppUserId == userId);
        ;

        return (await query.ToListAsync()).Select(u => Mapper.Map(u)!);
    }

    public async Task<DTO.UserSubscription?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.Subscription)
            .Include(u => u.AppUser);

        return Mapper.Map(await query.FirstOrDefaultAsync(u => u.Id == id));
    }
}