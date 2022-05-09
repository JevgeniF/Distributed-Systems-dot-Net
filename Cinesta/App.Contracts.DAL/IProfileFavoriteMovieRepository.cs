using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IProfileFavoriteMovieRepository : IEntityRepository<ProfileFavoriteMovie>
{
    Task<IEnumerable<DTO.ProfileFavoriteMovie>> IncludeGetAllByProfileIdAsync(Guid profileId,
        bool noTracking = true);

    Task<IEnumerable<DTO.ProfileFavoriteMovie>> IncludeGetAllAsync(bool noTracking = true);
    Task<DTO.ProfileFavoriteMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}