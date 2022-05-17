using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ProfileFavoriteMovieMapper : BaseMapper<ProfileFavoriteMovie, Domain.ProfileFavoriteMovie>
{
    public ProfileFavoriteMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}