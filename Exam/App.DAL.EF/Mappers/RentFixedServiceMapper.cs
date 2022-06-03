using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class RentFixedServiceMapper: BaseMapper<RentFixedService, Domain.RentFixedService>
{
    public RentFixedServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}