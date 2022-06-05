using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IApartAmenityRepository : IEntityRepository<ApartAmenity>
{
    Task<IEnumerable<ApartAmenity>> GetAllByApartId(Guid apartId, bool noTracking);
}