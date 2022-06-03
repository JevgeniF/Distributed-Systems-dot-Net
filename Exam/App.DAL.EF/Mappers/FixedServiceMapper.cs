using App.Domain;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class FixedServiceMapper: BaseMapper<App.DAL.DTO.FixedService, Domain.FixedService>
{
    public FixedServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}