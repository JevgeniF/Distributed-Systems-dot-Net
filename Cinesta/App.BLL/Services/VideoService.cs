using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class VideoService : BaseEntityService<Video, App.DAL.DTO.Video, IVideoRepository>, IVideoService
{
    public VideoService(IVideoRepository repository, IMapper<Video, DAL.DTO.Video> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Video>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Repository.IncludeGetAllAsync(noTracking)).Select(v => Mapper.Map(v)!);
    }

    public async Task<Video?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}