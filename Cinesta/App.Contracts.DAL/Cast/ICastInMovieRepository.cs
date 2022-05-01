using App.Domain.Cast;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Cast;

public interface ICastInMovieRepository : IEntityRepository<CastInMovie>
{
    Task<IEnumerable<CastInMovie>> GetWithInclude (bool noTracking = true);
    IQueryable<CastInMovie> QueryableWithInclude(bool noTracking = true);
}