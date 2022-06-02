using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IVideoService : IEntityService<Video>, IVideoRepositoryCustom<Video>
{
}

public interface IVideoServiceCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}