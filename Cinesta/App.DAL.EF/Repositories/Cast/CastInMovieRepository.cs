using App.Contracts.DAL.Cast;
using App.Domain.Cast;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Cast;

public class CastInMovieRepository : BaseEntityRepository<CastInMovie, AppDbContext>, ICastInMovieRepository
{
    public CastInMovieRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}