using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class PersonMapper : BaseMapper<Person, BLL.DTO.Person>
{
    public PersonMapper(IMapper mapper) : base(mapper)
    {
    }
}