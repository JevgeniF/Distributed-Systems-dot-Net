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

    public async Task<IEnumerable<UserRating>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Service.IncludeGetAllAsync(noTracking)).Select(u => Mapper.Map(u)!);
    }

    public async Task<UserRating?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}