using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Movie;

public class UserRatingRepository : BaseEntityRepository<UserRating, AppDbContext>, IUserRatingRepository
{
    public UserRatingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}