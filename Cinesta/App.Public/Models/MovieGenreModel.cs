using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class MovieGenreModel : BaseEntityModel<MovieGenre, BLL.DTO.MovieGenre, IMovieGenreService>,
    IMovieGenreModel
{
    public MovieGenreModel(IMovieGenreService service, IMapper<MovieGenre, BLL.DTO.MovieGenre> mapper) : base(
        service, mapper)
    {
    }

    public async Task<IEnumerable<MovieGenre>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Service.IncludeGetAllAsync(noTracking)).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieGenre?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}