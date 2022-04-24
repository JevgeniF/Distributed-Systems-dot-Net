using App.Contracts.DAL.MovieStandardDetails;
using App.Domain.MovieStandardDetails;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.MovieStandardDetails;

public class AgeRatingRepository : BaseEntityRepository<AgeRating, AppDbContext>, IAgeRatingRepository
{
    public AgeRatingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}