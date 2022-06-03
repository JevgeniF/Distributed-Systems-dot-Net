using App.Contracts.DAL;
using App.Domain;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class FixedServiceRepository : BaseEntityRepository<App.DAL.DTO.FixedService, Domain.FixedService, AppDbContext>,
    IFixedServiceRepository
{
    public FixedServiceRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.FixedService, FixedService> mapper) : base(dbContext, mapper)
    {
    }
}