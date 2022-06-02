using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IVideoRepository : IEntityRepository<Video>, IVideoRepositoryCustom<Video>
{
}

public interface IVideoRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}