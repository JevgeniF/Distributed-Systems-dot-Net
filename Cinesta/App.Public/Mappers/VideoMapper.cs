using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class VideoMapper : BaseMapper<Video, BLL.DTO.Video>
{
    public VideoMapper(IMapper mapper) : base(mapper)
    {
    }
}