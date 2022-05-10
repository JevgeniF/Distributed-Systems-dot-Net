using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class UserRatingService : BaseEntityService<UserRating, App.DAL.DTO.UserRating, IUserRatingRepository>,
    IUserRatingService
{
    public UserRatingService(IUserRatingRepository repository, IMapper<UserRating, DAL.DTO.UserRating> mapper) : base(
        repository, mapper)
    {
    }

    public async Task<IEnumerable<UserRating>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Repository.IncludeGetAllAsync(noTracking)).Select(u => Mapper.Map(u)!);
    }

    public async Task<UserRating?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}