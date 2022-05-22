using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class ProfileFavoriteMovieMapper : BaseMapper<ProfileFavoriteMovie, BLL.DTO.ProfileFavoriteMovie>
{
    public ProfileFavoriteMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}