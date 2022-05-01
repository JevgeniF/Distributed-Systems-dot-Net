using App.Contracts.DAL.Cast;
using App.Domain.Cast;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Cast;

public class CastInMovieRepository : BaseEntityRepository<CastInMovie, AppDbContext>, ICastInMovieRepository
{
    public CastInMovieRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<CastInMovie>> GetWithInclude(bool noTracking = true)
    {
        return await QueryableWithInclude().ToListAsync();
    }

    public IQueryable<CastInMovie> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(c => c.CastRole)
            .Include(c => c.MovieDetails).Include(c => c.Persons);
    }
}