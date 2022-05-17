using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ProfileMovieMapper : BaseMapper<ProfileMovie, Domain.ProfileMovie>
{
    public ProfileMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}