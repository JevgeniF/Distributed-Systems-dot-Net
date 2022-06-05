using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IApartRentRepository: IEntityRepository<ApartRent>
{
    Task<IEnumerable<ApartRent>> GetAllByApartId(Guid apartId, bool noTracking);
    Task<IEnumerable<ApartRent>> GetAllByPersonId(Guid personId, bool noTracking);
}