using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class CastInMovieMapper : BaseMapper<CastInMovie, BLL.DTO.CastInMovie>
{
    public CastInMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}