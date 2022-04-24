using App.Domain.Common;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Common;

public interface IPersonRepository: IEntityRepository<Person>
{
    // for custom methods
}