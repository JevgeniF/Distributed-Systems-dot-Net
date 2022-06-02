using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MovieTypeMapper : BaseMapper<MovieType, DAL.DTO.MovieType>
{
    public MovieTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}