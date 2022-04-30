using App.Domain.Profile;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Profile;

public interface IUserProfileRepository : IEntityRepository<UserProfile>
{
    Task<IEnumerable<UserProfile>> GetAllByUserIdAsync(Guid userId, bool noTracking = true);
}