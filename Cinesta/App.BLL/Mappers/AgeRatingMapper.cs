using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class AgeRatingMapper : BaseMapper<AgeRating, DAL.DTO.AgeRating>
{
    public AgeRatingMapper(IMapper mapper) : base(mapper)
    {
    }
}