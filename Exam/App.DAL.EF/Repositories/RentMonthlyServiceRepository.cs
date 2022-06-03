using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RentMonthlyServiceRepository : BaseEntityRepository<RentMonthlyService, Domain.RentMonthlyService, AppDbContext>,
    IRentMonthlyServiceRepository
{
    public RentMonthlyServiceRepository(AppDbContext dbContext, IMapper<RentMonthlyService, Domain.RentMonthlyService> mapper) : base(dbContext, mapper)
    {
        
    }

    public async Task<IEnumerable<RentMonthlyService>> GetAllByRentId(Guid rentId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.ApartRentId == rentId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }
}