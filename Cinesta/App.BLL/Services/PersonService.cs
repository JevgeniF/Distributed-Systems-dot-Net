using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class PersonService : BaseEntityService<Person, DAL.DTO.Person, IPersonRepository>, IPersonService
{
    public PersonService(IPersonRepository repository, IMapper<Person, DAL.DTO.Person> mapper) : base(repository,
        mapper)
    {
    }
}