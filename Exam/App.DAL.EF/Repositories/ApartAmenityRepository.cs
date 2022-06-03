using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ApartAmenityRepository : BaseEntityRepository<ApartAmenity, Domain.ApartAmenity, AppDbContext>,
    IApartAmenityRepository
{
    public ApartAmenityRepository(AppDbContext dbContext, IMapper<ApartAmenity, Domain.ApartAmenity> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<ApartAmenity>> GetAllByApartId(Guid apartId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.ApartmentId == apartId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }
}