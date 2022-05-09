using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class CastRoleRepository : BaseEntityRepository<DTO.CastRole, CastRole, AppDbContext>, ICastRoleRepository
{
    public CastRoleRepository(AppDbContext dbContext, IMapper<DTO.CastRole, CastRole> mapper) : base(dbContext, mapper)
    {
    }
}