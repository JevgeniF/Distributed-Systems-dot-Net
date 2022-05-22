using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class UserRatingModel : BaseEntityModel<UserRating, BLL.DTO.UserRating, IUserRatingService>,
    IUserRatingModel
{
    public UserRatingModel(IUserRatingService service, IMapper<UserRating, BLL.DTO.UserRating> mapper) : base(
        service, mapper)
    {
    }
}