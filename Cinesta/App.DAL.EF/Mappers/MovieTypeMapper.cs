using App.DAL.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MovieTypeMapper : BaseMapper<MovieType, Domain.MovieType>
{
    public MovieTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}