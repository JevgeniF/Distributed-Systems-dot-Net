using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MovieGenreMapper : BaseMapper<MovieGenre, DAL.DTO.MovieGenre>
{
    public MovieGenreMapper(IMapper mapper) : base(mapper)
    {
    }
}