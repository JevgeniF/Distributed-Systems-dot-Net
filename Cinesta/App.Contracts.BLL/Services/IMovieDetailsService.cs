using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMovieDetailsService : IEntityService<MovieDetails>, IMovieDetailsRepositoryCustom<MovieDetails>
{
}

public interface IMovieDetailsServiceCustom<TEntity>
{
    Task<IEnumerable<TEntity>> IncludeGetAllAsync(bool noTracking = true);
    Task<TEntity?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true);
    Task<IEnumerable<TEntity>> IncludeGetByAgeAsync(int age, bool noTracking = true);
}