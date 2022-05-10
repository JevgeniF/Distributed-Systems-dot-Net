using App.DAL.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MovieDetailsMapper : BaseMapper<MovieDetails, Domain.MovieDetails>
{
    public MovieDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}