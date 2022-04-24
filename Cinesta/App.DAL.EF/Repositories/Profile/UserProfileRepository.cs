using App.Contracts.DAL.Profile;
using App.Domain.Profile;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Profile;

public class UserProfileRepository: BaseEntityRepository<UserProfile, AppDbContext>, IUserProfileRepository
{
    public UserProfileRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}