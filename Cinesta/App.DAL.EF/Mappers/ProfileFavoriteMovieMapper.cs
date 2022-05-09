using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ProfileFavoriteMovieMapper : BaseMapper<ProfileFavoriteMovie, Domain.ProfileFavoriteMovie>
{
    public ProfileFavoriteMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}