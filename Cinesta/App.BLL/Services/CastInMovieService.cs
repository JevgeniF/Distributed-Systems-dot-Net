using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CastInMovieService: BaseEntityService<CastInMovie, App.DAL.DTO.CastInMovie, ICastInMovieRepository>, ICastInMovieService
{
    public CastInMovieService(ICastInMovieRepository repository, IMapper<CastInMovie, DAL.DTO.CastInMovie> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<CastInMovie>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Repository.IncludeGetAllAsync(noTracking)).Select(c => Mapper.Map(c)!);
    }

    public async Task<CastInMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}