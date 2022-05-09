using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IVideoRepository : IEntityRepository<Video>
{
    Task<IEnumerable<Video>> IncludeGetAllAsync(bool noTracking = true);
    Task<DTO.Video?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}