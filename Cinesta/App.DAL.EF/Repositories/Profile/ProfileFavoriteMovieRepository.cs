using App.Contracts.DAL.Profile;
using App.Domain.Profile;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Profile;

public class ProfileFavoriteMovieRepository : BaseEntityRepository<ProfileFavoriteMovie, AppDbContext>,
    IProfileFavoriteMovieRepository
{
    public ProfileFavoriteMovieRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> GetAllByProfileIdAsync(Guid profileId, bool noTracking = true)
    {
        var query = QueryableWithInclude()
            .Where(p => p.UserProfileId == profileId);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> GetWithInclude(bool noTracking = true)
    {
        return await QueryableWithInclude().ToListAsync();
    }

    public IQueryable<ProfileFavoriteMovie> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile);
    }
}