using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PersonRepository : BaseEntityRepository<Person, Domain.Person, AppDbContext>,
    IPersonRepository
{
    public PersonRepository(AppDbContext dbContext, IMapper<Person, Domain.Person> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Person?> GetByNames(string userName, string userSurname, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Where(p => p.FirstName == userName && p.LastName == userSurname);

        return Mapper.Map(await query.FirstOrDefaultAsync());
    }
}