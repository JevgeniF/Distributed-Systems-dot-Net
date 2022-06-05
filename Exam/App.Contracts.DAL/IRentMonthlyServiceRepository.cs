using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IRentMonthlyServiceRepository: IEntityRepository<RentMonthlyService>
{
    Task<IEnumerable<RentMonthlyService>> GetAllByRentId(Guid rentId, bool noTracking);
}