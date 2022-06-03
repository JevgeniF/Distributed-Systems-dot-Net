using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ApartPictureMapper: BaseMapper<ApartPicture, Domain.ApartPicture>
{
    public ApartPictureMapper(IMapper mapper) : base(mapper)
    { 
    }
}