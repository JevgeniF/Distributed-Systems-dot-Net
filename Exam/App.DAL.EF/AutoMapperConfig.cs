using App.DAL.DTO;
using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Subscription, Domain.Subscription>().ReverseMap();
        CreateMap<AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}