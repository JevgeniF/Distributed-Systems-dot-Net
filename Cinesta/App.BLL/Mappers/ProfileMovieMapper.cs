using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ProfileMovieMapper : BaseMapper<ProfileMovie, DAL.DTO.ProfileMovie>
{
    public ProfileMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}