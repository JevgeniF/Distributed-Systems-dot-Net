using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Microsoft.EntityFrameworkCore;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class UserRatingRepository : BaseEntityRepository<DTO.UserRating, UserRating, AppDbContext>,
    IUserRatingRepository
{
    public UserRatingRepository(AppDbContext dbContext, IMapper<DTO.UserRating, UserRating> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<DTO.UserRating>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser)
            .Include(u => u.MovieDetails);
        return (await query.ToListAsync()).Select(u => Mapper.Map(u)!);
    }
    
    public async Task<DTO.UserRating?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(u => u.AppUser)
            .Include(u => u.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(u => u.Id == id));
    }
}