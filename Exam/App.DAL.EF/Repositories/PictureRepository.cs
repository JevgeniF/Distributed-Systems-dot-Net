using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class PictureRepository : BaseEntityRepository<Picture, Domain.Picture, AppDbContext>,
    IPictureRepository
{
    public PictureRepository(AppDbContext dbContext, IMapper<Picture, Domain.Picture> mapper) : base(dbContext, mapper)
    {
    }
}