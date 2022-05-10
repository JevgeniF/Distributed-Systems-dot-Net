using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class MovieDbScoreService: BaseEntityService<MovieDbScore, App.DAL.DTO.MovieDbScore, IMovieDbScoreRepository>, IMovieDbScoreService
{
    public MovieDbScoreService(IMovieDbScoreRepository repository, IMapper<MovieDbScore, DAL.DTO.MovieDbScore> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<MovieDbScore>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Repository.IncludeGetAllAsync(noTracking)).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieDbScore?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id));
    }
}