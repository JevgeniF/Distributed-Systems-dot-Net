using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class AgeRatingService : BaseEntityService<AgeRating, DAL.DTO.AgeRating, IAgeRatingRepository>,
    IAgeRatingService
{
    public AgeRatingService(IAgeRatingRepository repository, IMapper<AgeRating, DAL.DTO.AgeRating> mapper) : base(
        repository, mapper)
    {
    }
}