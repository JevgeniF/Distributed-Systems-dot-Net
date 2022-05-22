using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class AgeRatingModel : BaseEntityModel<AgeRating, BLL.DTO.AgeRating, IAgeRatingService>,
    IAgeRatingModel
{
    public AgeRatingModel(IAgeRatingService service, IMapper<AgeRating, BLL.DTO.AgeRating> mapper) : base(
        service, mapper)
    {
    }
}