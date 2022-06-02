using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CastInMovieMapper : BaseMapper<CastInMovie, DAL.DTO.CastInMovie>
{
    public CastInMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}