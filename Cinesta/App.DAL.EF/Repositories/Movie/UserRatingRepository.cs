using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Movie;

public class UserRatingRepository : BaseEntityRepository<UserRating, AppDbContext>, IUserRatingRepository
{
    public UserRatingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<UserRating>> GetWithInclude(bool noTracking = true)
    {
        return await QueryableWithInclude().ToListAsync();
    }

    public IQueryable<UserRating> QueryableWithInclude(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return query.Include(u => u.AppUser)
            .Include(u => u.MovieDetails);
    }
}