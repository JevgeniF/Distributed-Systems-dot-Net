using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Movie;

public class VideoRepository : BaseEntityRepository<Video, AppDbContext>, IVideoRepository
{
    public VideoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Video>> GetWithInclude(bool noTracking = true)
    {
        return await QueryableWithInclude().ToListAsync();
    }

    public IQueryable<Video> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(v => v.MovieDetails);
    }
}