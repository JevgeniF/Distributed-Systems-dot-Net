using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CastInMovieMapper : BaseMapper<CastInMovie, Domain.CastInMovie>
{
    public CastInMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}