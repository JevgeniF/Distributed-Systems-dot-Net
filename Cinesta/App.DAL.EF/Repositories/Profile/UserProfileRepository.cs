using App.Contracts.DAL.Profile;
using App.Domain.Profile;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Profile;

public class UserProfileRepository : BaseEntityRepository<UserProfile, AppDbContext>, IUserProfileRepository
{
    public UserProfileRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<UserProfile>> GetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser).Where(u => u.AppUserId == userId);
        
        return await query.ToListAsync();
    }
}