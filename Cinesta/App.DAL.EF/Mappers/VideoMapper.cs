using App.DAL.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VideoMapper : BaseMapper<Video, Domain.Video>
{
    public VideoMapper(IMapper mapper) : base(mapper)
    {
    }
}