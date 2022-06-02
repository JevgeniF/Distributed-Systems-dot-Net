using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class MovieDetailsService : BaseEntityService<MovieDetails, DAL.DTO.MovieDetails, IMovieDetailsRepository>,
    IMovieDetailsService
{
    public MovieDetailsService(IMovieDetailsRepository repository, IMapper<MovieDetails, DAL.DTO.MovieDetails> mapper) :
        base(repository, mapper)
    {
    }

    public async Task<IEnumerable<MovieDetails>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Repository.IncludeGetAllAsync(noTracking)).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieDetails?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id, noTracking));
    }

    public async Task<IEnumerable<MovieDetails>> IncludeGetByAgeAsync(int age, bool noTracking = true)
    {
        return (await Repository.IncludeGetByAgeAsync(age)).Select(m => Mapper.Map(m)!);
    }
}