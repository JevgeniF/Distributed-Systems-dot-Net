using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ApartRentRepository : BaseEntityRepository<ApartRent, Domain.ApartRent, AppDbContext>,
    IApartRentRepository
{
    public ApartRentRepository(AppDbContext dbContext, IMapper<ApartRent, Domain.ApartRent> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<ApartRent>> GetAllByApartId(Guid apartId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.ApartmentId == apartId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }

    public async Task<IEnumerable<ApartRent>> GetAllByPersonId(Guid personId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.PersonId == personId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }
}