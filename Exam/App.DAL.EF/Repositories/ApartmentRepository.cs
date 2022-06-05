using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ApartmentRepository : BaseEntityRepository<Apartment, Domain.Apartment, AppDbContext>,
    IApartmentRepository
{
    public ApartmentRepository(AppDbContext dbContext, IMapper<Apartment, Domain.Apartment> mapper) : base(dbContext, mapper)
    {
    }
}