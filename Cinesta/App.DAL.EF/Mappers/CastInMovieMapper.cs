using App.Domain;
using AutoMapper;
using Base.Contracts;
using Base.DAL;
using CastInMovie = App.DTO.CastInMovie;

namespace App.DAL.EF.Mappers;

public class CastInMovieMapper : BaseMapper<CastInMovie, Domain.CastInMovie>
{
    public CastInMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}