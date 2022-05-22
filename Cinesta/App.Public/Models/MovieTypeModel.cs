using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class MovieTypeModel : BaseEntityModel<MovieType, BLL.DTO.MovieType, IMovieTypeService>,
    IMovieTypeModel
{
    public MovieTypeModel(IMovieTypeService service, IMapper<MovieType, BLL.DTO.MovieType> mapper) : base(
        service, mapper)
    {
    }
}