using App.Contracts.DAL.Common;
using App.Domain.Common;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Common;

public class PersonRepository : BaseEntityRepository<Person, AppDbContext>, IPersonRepository
{
    public PersonRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}