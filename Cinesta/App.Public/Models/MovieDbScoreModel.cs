using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class MovieDbScoreModel : BaseEntityModel<MovieDbScore, BLL.DTO.MovieDbScore, IMovieDbScoreService>,
    IMovieDbScoreModel
{
    public MovieDbScoreModel(IMovieDbScoreService service, IMapper<MovieDbScore, BLL.DTO.MovieDbScore> mapper) : base(
        service, mapper)
    {
    }

    public async Task<IEnumerable<MovieDbScore>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Service.IncludeGetAllAsync(noTracking)).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieDbScore?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeFirstOrDefaultAsync(id));
    }
}