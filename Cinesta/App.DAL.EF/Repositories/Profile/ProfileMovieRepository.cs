using App.Contracts.DAL.Profile;
using App.DAL.EF.Repositories.Movie;
using App.Domain.Profile;
using App.Resources.App.Domain.Movie;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using MovieDetails = App.Domain.Movie.MovieDetails;

namespace App.DAL.EF.Repositories.Profile;

public class ProfileMovieRepository : BaseEntityRepository<ProfileMovie, AppDbContext>, IProfileMovieRepository
{
    public ProfileMovieRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<IEnumerable<MovieDetails>> GetAllByProfileAgeAsync(int age, bool noTracking = true)
    {
        var repo = new MovieDetailsRepository(this.RepoDbContext);
        var query = repo.QueryableWithInclude()
            .Where(m => m.AgeRating!.AllowedAge <= age);
        
        return await query.ToListAsync();
    }
}