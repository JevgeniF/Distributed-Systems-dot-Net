using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class UserProfileService: BaseEntityService<UserProfile, DAL.DTO.UserProfile, IUserProfileRepository>, IUserProfileService
{
    public UserProfileService(IUserProfileRepository repository, IMapper<UserProfile, DAL.DTO.UserProfile> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<UserProfile>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.IncludeGetAllByUserIdAsync(userId, noTracking)).Select(u => Mapper.Map(u)!);
    }
}