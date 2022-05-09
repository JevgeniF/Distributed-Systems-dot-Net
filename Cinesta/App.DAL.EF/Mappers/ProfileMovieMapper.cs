using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ProfileMovieMapper : BaseMapper<ProfileMovie, Domain.ProfileMovie>
{
    public ProfileMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}