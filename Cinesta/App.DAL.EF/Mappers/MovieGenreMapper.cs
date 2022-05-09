using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MovieGenreMapper : BaseMapper<MovieGenre, Domain.MovieGenre>
{
    public MovieGenreMapper(IMapper mapper) : base(mapper)
    {
    }
}