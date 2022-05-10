using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IUserProfileService: IEntityService<UserProfile>, IUserProfileRepositoryCustom<UserProfile>
{
    
}