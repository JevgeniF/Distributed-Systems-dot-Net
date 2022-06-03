using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IApartInHouseRepository: IEntityRepository<ApartInHouse>
{
    Task<IEnumerable<ApartInHouse>> GetAllByHouseId(Guid houseId, bool noTracking);
}