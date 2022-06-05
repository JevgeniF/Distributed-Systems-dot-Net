using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MonthlyServiceMapper: BaseMapper<MonthlyService, Domain.MonthlyService>
{
    public MonthlyServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}