using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class ProfileMovieModel : BaseEntityModel<ProfileMovie, BLL.DTO.ProfileMovie, IProfileMovieService>,
    IProfileMovieModel
{
    public ProfileMovieModel(IProfileMovieService service, IMapper<ProfileMovie, BLL.DTO.ProfileMovie> mapper) : base(
        service, mapper)
    {
    }
}