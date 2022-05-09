using App.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPersonRepository : IEntityRepository<Person>
{
    Task<Person?> GetByNames(string userName, string userSurname, bool noTracking = true);
}