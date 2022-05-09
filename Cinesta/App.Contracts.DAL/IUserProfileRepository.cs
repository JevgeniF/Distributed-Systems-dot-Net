using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUserProfileRepository : IEntityRepository<UserProfile>
{
    Task<IEnumerable<UserProfile>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true);
}