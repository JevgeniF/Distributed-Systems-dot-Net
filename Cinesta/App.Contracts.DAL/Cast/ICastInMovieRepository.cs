using App.Domain.Cast;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Cast;

public interface ICastInMovieRepository : IEntityRepository<CastInMovie>
{
    //Task<IEnumerable<CastRole>> GetAllByUserIdAsync(bool noTracking = true);
}