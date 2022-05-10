using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AgeRatingMapper : BaseMapper<AgeRating, Domain.AgeRating>
{
    public AgeRatingMapper(IMapper mapper) : base(mapper)
    {
    }
}