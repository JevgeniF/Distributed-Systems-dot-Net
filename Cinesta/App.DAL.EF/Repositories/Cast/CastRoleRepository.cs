using App.Contracts.DAL.Cast;
using App.Domain.Cast;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Cast;

public class CastRoleRepository : BaseEntityRepository<CastRole, AppDbContext>, ICastRoleRepository
{
    public CastRoleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}