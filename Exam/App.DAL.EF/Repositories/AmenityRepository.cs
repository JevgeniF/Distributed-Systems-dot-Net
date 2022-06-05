using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class AmenityRepository : BaseEntityRepository<Amenity, Domain.Amenity, AppDbContext>,
    IAmenityRepository
{
    public AmenityRepository(AppDbContext dbContext, IMapper<Amenity, Domain.Amenity> mapper) : base(dbContext, mapper)
    {
    }
}