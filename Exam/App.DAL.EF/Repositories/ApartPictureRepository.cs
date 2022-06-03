using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ApartPictureRepository : BaseEntityRepository<ApartPicture, Domain.ApartPicture, AppDbContext>,
    IApartPictureRepository
{
    public ApartPictureRepository(AppDbContext dbContext, IMapper<ApartPicture, Domain.ApartPicture> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<ApartPicture>> GetAllByApartId(Guid apartId, bool noTracking)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.ApartmentId == apartId).ToListAsync()).Select(a => Mapper.Map(a))!;
    }
}