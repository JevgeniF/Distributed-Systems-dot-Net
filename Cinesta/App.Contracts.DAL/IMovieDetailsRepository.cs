using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMovieDetailsRepository : IEntityRepository<MovieDetails>, IMovieDetailsRepositoryCustom<MovieDetails>
{
}

public interface IMovieDetailsRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
    Task<IEnumerable<TEntity>> IncludeGetByAgeAsync(int age, bool noTracking = true);
}