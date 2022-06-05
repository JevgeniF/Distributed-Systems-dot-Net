using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class HouseMapper: BaseMapper<House, Domain.House>
{
    public HouseMapper(IMapper mapper) : base(mapper)
    {
    }
}