using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IBillingRepository: IEntityRepository<Billing>
{
    Task<IEnumerable<Billing>> GetAllByRentId(Guid apartId, bool noTracking);
    Task<IEnumerable<Billing>> GetAllByPersonId(Guid personId, bool noTracking);
}