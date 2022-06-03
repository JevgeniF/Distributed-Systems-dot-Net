using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ApartRentMapper: BaseMapper<ApartRent, Domain.ApartRent>
{
    public ApartRentMapper(IMapper mapper) : base(mapper)
    {
    }
}