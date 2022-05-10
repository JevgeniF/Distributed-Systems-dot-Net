using AutoMapper;
using Base.DAL;
using App.BLL.DTO;

namespace App.BLL.Mappers;

public class CastInMovieMapper : BaseMapper<CastInMovie, DAL.DTO.CastInMovie>
{
    public CastInMovieMapper(IMapper mapper) : base(mapper)
    {
    }
}