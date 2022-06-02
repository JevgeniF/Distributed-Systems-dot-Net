using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProfileFavoriteMovieRepository :
    BaseEntityRepository<ProfileFavoriteMovie, Domain.ProfileFavoriteMovie, AppDbContext>,
    IProfileFavoriteMovieRepository
{
    public ProfileFavoriteMovieRepository(AppDbContext dbContext,
        IMapper<ProfileFavoriteMovie, Domain.ProfileFavoriteMovie> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> IncludeGetAllByProfileIdAsync(Guid profileId,
        bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile)
            .Where(p => p.UserProfileId == profileId);

        return (await query.ToListAsync()).Select(p => Mapper.Map(p)!);
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile);

        return (await query.ToListAsync()).Select(p => Mapper.Map(p)!);
    }

    public async Task<ProfileFavoriteMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile);

        return Mapper.Map(await query.FirstOrDefaultAsync(p => p.Id == id));
    }
}