using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class MovieDetailsModel : BaseEntityModel<MovieDetails, BLL.DTO.MovieDetails, IMovieDetailsService>,
    IMovieDetailsModel
{
    public MovieDetailsModel(IMovieDetailsService service, IMapper<MovieDetails, BLL.DTO.MovieDetails> mapper) : base(
        service, mapper)
    {
    }

    public async Task<IEnumerable<MovieDetails>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Service.IncludeGetAllAsync(noTracking)).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieDetails?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeFirstOrDefaultAsync(id, noTracking));
    }

    public async Task<IEnumerable<MovieDetails>> IncludeGetByAgeAsync(int age, bool noTracking = true)
    {
        return (await Service.IncludeGetByAgeAsync(age)).Select(m => Mapper.Map(m)!);
    }
}