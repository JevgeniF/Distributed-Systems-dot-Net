using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class CastInMovieModel : BaseEntityModel<CastInMovie, BLL.DTO.CastInMovie, ICastInMovieService>,
    ICastInMovieModel
{
    public CastInMovieModel(ICastInMovieService service, IMapper<CastInMovie, BLL.DTO.CastInMovie> mapper) : base(
        service, mapper)
    {
    }

    public async Task<IEnumerable<CastInMovie>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Service.IncludeGetAllAsync(noTracking)).Select(c => Mapper.Map(c)!);
    }

    public async Task<CastInMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}