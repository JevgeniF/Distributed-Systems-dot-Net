using App.Domain.Common;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Common;

public interface IPersonRepository : IEntityRepository<Person>
{
    Task<Person?> GetByNames(string userName, string userSurname, bool noTracking = true);
}