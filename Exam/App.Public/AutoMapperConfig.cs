using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using AutoMapper;

namespace App.Public;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Subscription, BLL.DTO.Subscription>().ReverseMap();
        CreateMap<AppUser, BLL.DTO.Identity.AppUser>().ReverseMap();
    }
}