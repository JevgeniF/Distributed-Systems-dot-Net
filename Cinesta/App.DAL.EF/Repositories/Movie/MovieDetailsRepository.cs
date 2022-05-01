using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Movie;

public class MovieDetailsRepository : BaseEntityRepository<MovieDetails, AppDbContext>, IMovieDetailsRepository
{
    public MovieDetailsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<MovieDetails>> GetWithInclude(bool noTracking = true)
    {
        return await QueryableWithInclude().ToListAsync();
    }

    public async Task<IEnumerable<MovieDetails>> GetByAgeRating(int age, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return await query.Where(m => m.AgeRating!.AllowedAge <= age).ToListAsync();
    }

    public IQueryable<MovieDetails> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(m => m.AgeRating)
            .Include(m => m.MovieType);
    }
}