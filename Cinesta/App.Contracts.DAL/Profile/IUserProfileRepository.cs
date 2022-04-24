using App.Domain.Profile;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Profile;

public interface IUserProfileRepository: IEntityRepository<UserProfile>
{
    
}