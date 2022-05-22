using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class ProfileMovieMapper : BaseMapper<ProfileMovie, BLL.DTO.ProfileMovie>
{
    public ProfileMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}