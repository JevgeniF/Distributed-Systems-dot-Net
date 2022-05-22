using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class ProfileFavoriteMovieModel :
    BaseEntityModel<ProfileFavoriteMovie, BLL.DTO.ProfileFavoriteMovie, IProfileFavoriteMovieService>,
    IProfileFavoriteMovieModel
{
    public ProfileFavoriteMovieModel(IProfileFavoriteMovieService service,
        IMapper<ProfileFavoriteMovie, BLL.DTO.ProfileFavoriteMovie> mapper) : base(
        service, mapper)
    {
    }
}