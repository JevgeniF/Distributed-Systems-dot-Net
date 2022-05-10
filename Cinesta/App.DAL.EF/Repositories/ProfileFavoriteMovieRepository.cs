using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Microsoft.EntityFrameworkCore;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ProfileFavoriteMovieRepository :
    BaseEntityRepository<DTO.ProfileFavoriteMovie, ProfileFavoriteMovie, AppDbContext>,
    IProfileFavoriteMovieRepository
{
    public ProfileFavoriteMovieRepository(AppDbContext dbContext,
        IMapper<DTO.ProfileFavoriteMovie, ProfileFavoriteMovie> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<DTO.ProfileFavoriteMovie>> IncludeGetAllByProfileIdAsync(Guid profileId,
        bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile)
            .Where(p => p.UserProfileId == profileId);

        return (await query.ToListAsync()).Select(p => Mapper.Map(p)!);
    }

    public async Task<IEnumerable<DTO.ProfileFavoriteMovie>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile);

        return (await query.ToListAsync()).Select(p => Mapper.Map(p)!);
    }

    public async Task<DTO.ProfileFavoriteMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile);

        return Mapper.Map(await query.FirstOrDefaultAsync(p => p.Id == id));
    }
}