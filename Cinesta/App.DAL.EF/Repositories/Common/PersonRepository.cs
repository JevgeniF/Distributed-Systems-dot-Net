using App.Contracts.DAL.Common;
using App.Domain.Common;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Common;

public class PersonRepository : BaseEntityRepository<Person, AppDbContext>, IPersonRepository
{
    public PersonRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Person?> GetByNames(string userName, string userSurname, bool noTracking = true)
    {
        IQueryable<Person?> query = CreateQuery(noTracking);
        query = query.Where(p => p!.Name == userName);

        return await query.FirstOrDefaultAsync(p => p!.Surname == userSurname);
    }
}