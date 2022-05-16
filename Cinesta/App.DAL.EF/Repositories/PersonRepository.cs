using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.Contracts.Mapper;
using Microsoft.EntityFrameworkCore;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class PersonRepository : BaseEntityRepository<DTO.Person, Person, AppDbContext>, IPersonRepository
{
    public PersonRepository(AppDbContext dbContext, IMapper<DTO.Person, Person> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<DTO.Person?> GetByNames(string userName, string userSurname, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Where(p => p!.Name == userName);

        return Mapper.Map(await query.FirstOrDefaultAsync(p => p!.Surname == userSurname));
    }
}