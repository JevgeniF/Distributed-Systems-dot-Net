using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class PersonService : BaseEntityService<Person, App.DAL.DTO.Person, IPersonRepository>, IPersonService
{
    public PersonService(IPersonRepository repository, IMapper<Person, DAL.DTO.Person> mapper) : base(repository,
        mapper)
    {
    }
}