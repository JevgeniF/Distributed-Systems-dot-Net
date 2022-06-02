using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IUserProfileService : IEntityService<UserProfile>, IUserProfileRepositoryCustom<UserProfile>
{
}

public interface IUserProfileServiceCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true);
}