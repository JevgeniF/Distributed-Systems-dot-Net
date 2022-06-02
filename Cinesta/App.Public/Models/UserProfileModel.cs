using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class UserProfileModel : BaseEntityModel<UserProfile, BLL.DTO.UserProfile, IUserProfileService>,
    IUserProfileModel
{
    public UserProfileModel(IUserProfileService service, IMapper<UserProfile, BLL.DTO.UserProfile> mapper) : base(
        service, mapper)
    {
    }

    public async Task<IEnumerable<UserProfile>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return (await Service.IncludeGetAllByUserIdAsync(userId, noTracking)).Select(u => Mapper.Map(u)!);
    }
}