using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class MovieGenreMapper : BaseMapper<MovieGenre, BLL.DTO.MovieGenre>
{
    public MovieGenreMapper(IMapper mapper) : base(mapper)
    {
    }
}