using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BillingRepository : BaseEntityRepository<Billing, Domain.Billing, AppDbContext>,
    IBillingRepository
{
    public BillingRepository(AppDbContext dbContext, IMapper<Billing, Domain.Billing> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Billing>> GetAllByRentId(Guid rentId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.ApartRentId == rentId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }

    public async Task<IEnumerable<Billing>> GetAllByPersonId(Guid personId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.PersonId == personId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }
}