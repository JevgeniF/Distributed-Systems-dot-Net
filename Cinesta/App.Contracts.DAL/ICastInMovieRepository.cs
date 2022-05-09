using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface ICastInMovieRepository : IEntityRepository<CastInMovie>
{
    Task<IEnumerable<CastInMovie>> IncludeGetAllAsync(bool noTracking = true);
    Task<CastInMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
}