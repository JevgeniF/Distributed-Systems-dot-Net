using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class AgeRatingMapper : BaseMapper<AgeRating, BLL.DTO.AgeRating>
{
    public AgeRatingMapper(IMapper mapper) : base(mapper)
    {
    }
}