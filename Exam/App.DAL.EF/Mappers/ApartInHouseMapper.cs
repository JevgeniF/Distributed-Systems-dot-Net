using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ApartInHouseMapper: BaseMapper<ApartInHouse, Domain.ApartInHouse>
{
    public ApartInHouseMapper(IMapper mapper) : base(mapper)
    {
    }
}