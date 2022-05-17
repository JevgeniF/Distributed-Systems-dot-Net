using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UserRatingRepository : BaseEntityRepository<UserRating, Domain.UserRating, AppDbContext>,
    IUserRatingRepository
{
    public UserRatingRepository(AppDbContext dbContext, IMapper<UserRating, Domain.UserRating> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<UserRating>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser)
            .Include(u => u.MovieDetails);
        return (await query.ToListAsync()).Select(u => Mapper.Map(u)!);
    }

    public async Task<UserRating?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser)
            .Include(u => u.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(u => u.Id == id));
    }
}