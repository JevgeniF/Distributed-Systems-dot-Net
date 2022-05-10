using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class VideoMapper : BaseMapper<Video, DAL.DTO.Video>
{
    public VideoMapper(IMapper mapper) : base(mapper)
    {
    }
}