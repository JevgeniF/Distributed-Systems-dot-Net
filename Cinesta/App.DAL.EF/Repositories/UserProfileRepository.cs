using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UserProfileRepository : BaseEntityRepository<DTO.UserProfile, UserProfile, AppDbContext>,
    IUserProfileRepository
{
    public UserProfileRepository(AppDbContext dbContext, IMapper<DTO.UserProfile, UserProfile> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<DTO.UserProfile>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser).Where(u => u.AppUserId == userId);

        return (await query.ToListAsync()).Select(u => Mapper.Map(u)!);
    }
}