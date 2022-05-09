using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUserRatingRepository : IEntityRepository<UserRating>
{
    Task<IEnumerable<UserRating>> IncludeGetAllAsync(bool noTracking = true);
    Task<DTO.UserRating?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}