using App.Domain.Profile;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Profile;

public interface IProfileFavoriteMovieRepository : IEntityRepository<ProfileFavoriteMovie>
{
    Task<IEnumerable<ProfileFavoriteMovie>> GetAllByProfileIdAsync(Guid profileId, bool noTracking = true);
    Task<IEnumerable<ProfileFavoriteMovie>> GetWithInclude(bool noTracking = true);
    IQueryable<ProfileFavoriteMovie> QueryableWithInclude(bool noTracking = true);
}