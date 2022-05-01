using App.Contracts.DAL.Profile;
using App.DAL.EF.Repositories.Movie;
using App.Domain.Movie;
using App.Domain.Profile;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Profile;

public class ProfileMovieRepository : BaseEntityRepository<ProfileMovie, AppDbContext>, IProfileMovieRepository
{
    public ProfileMovieRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<MovieDetails>> GetAllByProfileAgeAsync(int age, bool noTracking = true)
    {
        var repo = new MovieDetailsRepository(RepoDbContext);
        var query = repo.QueryableWithInclude()
            .Where(m => m.AgeRating!.AllowedAge <= age);

        return await query.ToListAsync();
    }
}