using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PictureMapper: BaseMapper<Picture, Domain.Picture>
{
    public PictureMapper(IMapper mapper) : base(mapper)
    {
    }
}