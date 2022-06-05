using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ApartAmenityMapper: BaseMapper<ApartAmenity, Domain.ApartAmenity>
{
    public ApartAmenityMapper(IMapper mapper) : base(mapper)
    {
    }
}