using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class MovieTypeMapper : BaseMapper<MovieType, BLL.DTO.MovieType>
{
    public MovieTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}