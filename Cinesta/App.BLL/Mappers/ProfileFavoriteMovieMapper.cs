using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ProfileFavoriteMovieMapper : BaseMapper<ProfileFavoriteMovie, DAL.DTO.ProfileFavoriteMovie>
{
    public ProfileFavoriteMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}