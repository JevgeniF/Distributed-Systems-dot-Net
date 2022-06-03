using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IApartPictureRepository: IEntityRepository<ApartPicture>
{
    Task<IEnumerable<ApartPicture>> GetAllByApartId(Guid apartId, bool noTracking);
}