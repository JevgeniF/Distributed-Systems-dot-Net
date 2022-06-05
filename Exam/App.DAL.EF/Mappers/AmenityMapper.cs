using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AmenityMapper : BaseMapper<Amenity, Domain.Amenity>
{
    public AmenityMapper(IMapper mapper) : base(mapper)
    {
    }
}