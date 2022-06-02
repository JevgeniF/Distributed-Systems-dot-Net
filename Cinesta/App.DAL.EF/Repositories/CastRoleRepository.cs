using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class CastRoleRepository : BaseEntityRepository<CastRole, Domain.CastRole, AppDbContext>, ICastRoleRepository
{
    public CastRoleRepository(AppDbContext dbContext, IMapper<CastRole, Domain.CastRole> mapper) : base(dbContext,
        mapper)
    {
    }
}