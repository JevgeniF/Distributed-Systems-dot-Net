using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUserProfileRepository : IEntityRepository<UserProfile>, IUserProfileRepositoryCustom<UserProfile>
{
}

public interface IUserProfileRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true);
}