using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IRentFixedServiceRepository: IEntityRepository<RentFixedService>
{
    Task<IEnumerable<RentFixedService>> GetAllByRentId(Guid rentId, bool noTracking);
}