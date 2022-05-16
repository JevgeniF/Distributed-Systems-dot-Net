using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class AgeRatingRepository : BaseEntityRepository<DTO.AgeRating, AgeRating, AppDbContext>,
    IAgeRatingRepository
{
    public AgeRatingRepository(AppDbContext dbContext, IMapper<DTO.AgeRating, AgeRating> mapper) : base(dbContext,
        mapper)
    {
    }
}