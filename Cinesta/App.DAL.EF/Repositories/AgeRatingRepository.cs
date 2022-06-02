using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class AgeRatingRepository : BaseEntityRepository<AgeRating, Domain.AgeRating, AppDbContext>,
    IAgeRatingRepository
{
    public AgeRatingRepository(AppDbContext dbContext, IMapper<AgeRating, Domain.AgeRating> mapper) : base(dbContext,
        mapper)
    {
    }
}