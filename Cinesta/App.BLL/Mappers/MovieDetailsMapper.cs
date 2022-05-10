using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MovieDetailsMapper : BaseMapper<MovieDetails, DAL.DTO.MovieDetails>
{
    public MovieDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}