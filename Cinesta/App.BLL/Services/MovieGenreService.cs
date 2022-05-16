using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class MovieGenreService : BaseEntityService<MovieGenre, App.DAL.DTO.MovieGenre, IMovieGenreRepository>,
    IMovieGenreService
{
    public MovieGenreService(IMovieGenreRepository repository, IMapper<MovieGenre, DAL.DTO.MovieGenre> mapper) : base(
        repository, mapper)
    {
    }

    public async Task<IEnumerable<MovieGenre>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Repository.IncludeGetAllAsync(noTracking)).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieGenre?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}