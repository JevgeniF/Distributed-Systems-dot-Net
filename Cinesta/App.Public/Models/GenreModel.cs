using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class GenreModel : BaseEntityModel<Genre, BLL.DTO.Genre, IGenreService>,
    IGenreModel
{
    public GenreModel(IGenreService service, IMapper<Genre, BLL.DTO.Genre> mapper) : base(
        service, mapper)
    {
    }
}