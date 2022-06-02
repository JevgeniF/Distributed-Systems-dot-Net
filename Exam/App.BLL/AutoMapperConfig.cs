using App.BLL.DTO;
using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Subscription, DAL.DTO.Subscription>().ReverseMap();
    }
}