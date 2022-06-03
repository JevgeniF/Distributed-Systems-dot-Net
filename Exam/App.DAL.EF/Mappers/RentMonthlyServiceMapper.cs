using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class RentMonthlyServiceMapper: BaseMapper<RentMonthlyService, Domain.RentMonthlyService>
{
    public RentMonthlyServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}