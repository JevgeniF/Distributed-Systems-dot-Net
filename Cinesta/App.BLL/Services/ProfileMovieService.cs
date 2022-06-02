using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class ProfileMovieService : BaseEntityService<ProfileMovie, DAL.DTO.ProfileMovie, IProfileMovieRepository>,
    IProfileMovieService
{
    public ProfileMovieService(IProfileMovieRepository repository, IMapper<ProfileMovie, DAL.DTO.ProfileMovie> mapper) :
        base(repository, mapper)
    {
    }
}