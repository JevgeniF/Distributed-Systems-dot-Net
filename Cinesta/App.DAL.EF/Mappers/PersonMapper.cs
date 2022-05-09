using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PersonMapper : BaseMapper<Person, Domain.Person>
{
    public PersonMapper(IMapper mapper) : base(mapper)
    {
    }
}