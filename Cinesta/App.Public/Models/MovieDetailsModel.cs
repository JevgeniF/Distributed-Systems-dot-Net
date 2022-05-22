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
}