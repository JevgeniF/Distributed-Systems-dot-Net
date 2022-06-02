using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class MovieDbScoreMapper : BaseMapper<MovieDbScore, BLL.DTO.MovieDbScore>
{
    public MovieDbScoreMapper(IMapper mapper) : base(mapper)
    {
    }
}