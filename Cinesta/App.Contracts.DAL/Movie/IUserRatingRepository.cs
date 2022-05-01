using App.Domain.Movie;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Movie;

public interface IUserRatingRepository : IEntityRepository<UserRating>
{
    Task<IEnumerable<UserRating>> GetWithInclude (bool noTracking = true);
    IQueryable<UserRating> QueryableWithInclude(bool noTracking = true);
}