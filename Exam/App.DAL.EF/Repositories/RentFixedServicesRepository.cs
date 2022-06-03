using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RentFixedServicesRepository : BaseEntityRepository<RentFixedService, Domain.RentFixedService, AppDbContext>,
    IRentFixedServiceRepository
{
    public RentFixedServicesRepository(AppDbContext dbContext, IMapper<RentFixedService, Domain.RentFixedService> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<RentFixedService>> GetAllByRentId(Guid rentId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.ApartRentId == rentId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }
}