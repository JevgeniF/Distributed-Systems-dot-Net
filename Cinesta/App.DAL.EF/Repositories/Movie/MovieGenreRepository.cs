using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Movie;

public class MovieGenreRepository : BaseEntityRepository<MovieGenre, AppDbContext>, IMovieGenreRepository
{
    public MovieGenreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<MovieGenre>> GetWithInclude(bool noTracking = true)
    {
        return await QueryableWithInclude().ToListAsync();
    }

    public IQueryable<MovieGenre> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(m => m.Genre)
            .Include(m => m.MovieDetails);
    }
}