using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UserProfileRepository : BaseEntityRepository<UserProfile, Domain.UserProfile, AppDbContext>,
    IUserProfileRepository
{
    public UserProfileRepository(AppDbContext dbContext, IMapper<UserProfile, Domain.UserProfile> mapper) : base(
        dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<UserProfile>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser).Where(u => u.AppUserId == userId);

        return (await query.ToListAsync()).Select(u => Mapper.Map(u)!);
    }
}