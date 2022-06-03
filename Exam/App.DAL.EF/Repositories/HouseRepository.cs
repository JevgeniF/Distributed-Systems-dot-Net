using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class HouseRepository : BaseEntityRepository<House, Domain.House, AppDbContext>,
    IHouseRepository
{
    public HouseRepository(AppDbContext dbContext, IMapper<House, Domain.House> mapper) : base(dbContext, mapper)
    {
    }
}