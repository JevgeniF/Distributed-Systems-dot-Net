using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class PersonModel : BaseEntityModel<Person, BLL.DTO.Person, IPersonService>,
    IPersonModel
{
    public PersonModel(IPersonService service, IMapper<Person, BLL.DTO.Person> mapper) : base(
        service, mapper)
    {
    }

    public async Task<Person?> GetByNames(string userName, string userSurname, bool noTracking = true)
    {
        return Mapper.Map(await Service.GetByNames(userName, userSurname, noTracking));
    }
}