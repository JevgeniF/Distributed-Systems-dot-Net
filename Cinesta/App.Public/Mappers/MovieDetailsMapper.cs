using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class MovieDetailsMapper : BaseMapper<MovieDetails, BLL.DTO.MovieDetails>
{
    public MovieDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}