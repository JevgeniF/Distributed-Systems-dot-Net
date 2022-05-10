using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MovieDbScoreMapper : BaseMapper<MovieDbScore, DAL.DTO.MovieDbScore>
{
    public MovieDbScoreMapper(IMapper mapper) : base(mapper)
    {
    }
}