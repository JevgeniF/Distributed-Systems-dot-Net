using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPersonRepository : IEntityRepository<Person>, IPersonRepositoryCustom<Person>
{
}

public interface IPersonRepositoryCustom<TEntity>
{
    Task<TEntity?> GetByNames(string userName, string userSurname, bool noTracking = true);
}