using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class VideoModel : BaseEntityModel<Video, BLL.DTO.Video, IVideoService>,
    IVideoModel
{
    public VideoModel(IVideoService service, IMapper<Video, BLL.DTO.Video> mapper) : base(
        service, mapper)
    {
    }

    public async Task<IEnumerable<Video>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Service.IncludeGetAllAsync(noTracking)).Select(v => Mapper.Map(v)!);
    }

    public async Task<Video?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}