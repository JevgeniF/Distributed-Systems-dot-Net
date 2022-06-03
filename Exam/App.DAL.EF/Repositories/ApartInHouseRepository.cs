using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ApartInHouseRepository: BaseEntityRepository<ApartInHouse, Domain.ApartInHouse, AppDbContext>, IApartInHouseRepository
{
    public ApartInHouseRepository(AppDbContext dbContext, IMapper<ApartInHouse, Domain.ApartInHouse> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<ApartInHouse>> GetAllByHouseId(Guid houseId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.HouseId == houseId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }
}
