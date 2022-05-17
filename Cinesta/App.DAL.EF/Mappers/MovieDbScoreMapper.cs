using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MovieDbScoreMapper : BaseMapper<MovieDbScore, Domain.MovieDbScore>
{
    public MovieDbScoreMapper(IMapper mapper) : base(mapper)
    {
    }
}