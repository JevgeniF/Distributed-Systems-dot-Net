using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class MovieDbScoreService : BaseEntityService<MovieDbScore, DAL.DTO.MovieDbScore, IMovieDbScoreRepository>,
    IMovieDbScoreService
{
    public MovieDbScoreService(IMovieDbScoreRepository repository, IMapper<MovieDbScore, DAL.DTO.MovieDbScore> mapper) :
        base(repository, mapper)
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

    public async Task<MovieDbScore?> GetMovieDbScoresForMovie(Guid movieId, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GetMovieDbScoresForMovie(movieId));
    }
}