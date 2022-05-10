using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class MovieTypeService : BaseEntityService<MovieType, App.DAL.DTO.MovieType, IMovieTypeRepository>,
    IMovieTypeService
{
    public MovieTypeService(IMovieTypeRepository repository, IMapper<MovieType, DAL.DTO.MovieType> mapper) : base(
        repository, mapper)
    {
    }
}