using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Movie;

public class MovieDBScoreRepository : BaseEntityRepository<MovieDbScore, AppDbContext>, IMovieDBScoreRepository
{
    public MovieDBScoreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<MovieDbScore>> GetWithInclude(bool noTracking = true)
    {
        return await QueryableWithInclude().ToListAsync();
    }

    public IQueryable<MovieDbScore> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(m => m.MovieDetails);
    }
}