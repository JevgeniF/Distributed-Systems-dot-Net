using App.DAL.DTO;
using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Amenity, Domain.Amenity>().ReverseMap();
        CreateMap<ApartAmenity, Domain.ApartAmenity>().ReverseMap();
        CreateMap<Apartment, Domain.Apartment>().ReverseMap();
        CreateMap<ApartPicture, Domain.ApartPicture>().ReverseMap();
        CreateMap<ApartRent, Domain.ApartRent>().ReverseMap();
        CreateMap<Billing, Domain.Billing>().ReverseMap();
        CreateMap<App.DAL.DTO.FixedService, Domain.FixedService>().ReverseMap();
        CreateMap<House, Domain.House>().ReverseMap();
        CreateMap<Person, Domain.Person>().ReverseMap();
        CreateMap<Picture, Domain.Picture>().ReverseMap();
        CreateMap<RentFixedService, Domain.RentFixedService>().ReverseMap();
        CreateMap<RentMonthlyService, Domain.RentMonthlyService>().ReverseMap();
        CreateMap<MonthlyService, Domain.MonthlyService>().ReverseMap();
        CreateMap<RentFixedService, Domain.RentFixedService>().ReverseMap();
        CreateMap<AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}