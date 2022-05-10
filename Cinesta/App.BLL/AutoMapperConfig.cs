using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<DTO.AgeRating, App.DAL.DTO.AgeRating>().ReverseMap();
        CreateMap<DTO.CastInMovie, App.DAL.DTO.CastInMovie>().ReverseMap();
        CreateMap<DTO.CastRole, App.DAL.DTO.CastRole>().ReverseMap();
        CreateMap<DTO.Genre, App.DAL.DTO.Genre>().ReverseMap();
        CreateMap<DTO.MovieDbScore, App.DAL.DTO.MovieDbScore>().ReverseMap();
        CreateMap<DTO.MovieDetails, App.DAL.DTO.MovieDetails>().ReverseMap();
        CreateMap<DTO.MovieGenre, App.DAL.DTO.MovieGenre>().ReverseMap();
        CreateMap<DTO.MovieType, App.DAL.DTO.MovieType>().ReverseMap();
        CreateMap<DTO.PaymentDetails, App.DAL.DTO.PaymentDetails>().ReverseMap();
        CreateMap<DTO.Person, App.DAL.DTO.Person>().ReverseMap();
        CreateMap<DTO.ProfileFavoriteMovie, App.DAL.DTO.ProfileFavoriteMovie>().ReverseMap();
        CreateMap<DTO.ProfileMovie, App.DAL.DTO.ProfileMovie>().ReverseMap();
        CreateMap<DTO.Subscription, App.DAL.DTO.Subscription>().ReverseMap();
        CreateMap<DTO.UserProfile, App.DAL.DTO.UserProfile>().ReverseMap();
        CreateMap<DTO.UserRating, App.DAL.DTO.UserRating>().ReverseMap();
        CreateMap<DTO.UserSubscription, App.DAL.DTO.UserSubscription>().ReverseMap();
        CreateMap<DTO.Video, App.DAL.DTO.Video>().ReverseMap();
        CreateMap<DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
    }
}