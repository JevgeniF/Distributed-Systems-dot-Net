using App.BLL.DTO;
using App.BLL.DTO.Identity;
using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<AgeRating, DAL.DTO.AgeRating>().ReverseMap();
        CreateMap<CastInMovie, DAL.DTO.CastInMovie>().ReverseMap();
        CreateMap<CastRole, DAL.DTO.CastRole>().ReverseMap();
        CreateMap<Genre, DAL.DTO.Genre>().ReverseMap();
        CreateMap<MovieDbScore, DAL.DTO.MovieDbScore>().ReverseMap();
        CreateMap<MovieDetails, DAL.DTO.MovieDetails>().ReverseMap();
        CreateMap<MovieGenre, DAL.DTO.MovieGenre>().ReverseMap();
        CreateMap<MovieType, DAL.DTO.MovieType>().ReverseMap();
        CreateMap<PaymentDetails, DAL.DTO.PaymentDetails>().ReverseMap();
        CreateMap<Person, DAL.DTO.Person>().ReverseMap();
        CreateMap<ProfileFavoriteMovie, DAL.DTO.ProfileFavoriteMovie>().ReverseMap();
        CreateMap<ProfileMovie, DAL.DTO.ProfileMovie>().ReverseMap();
        CreateMap<Subscription, DAL.DTO.Subscription>().ReverseMap();
        CreateMap<UserProfile, DAL.DTO.UserProfile>().ReverseMap();
        CreateMap<UserRating, DAL.DTO.UserRating>().ReverseMap();
        CreateMap<UserSubscription, DAL.DTO.UserSubscription>().ReverseMap();
        CreateMap<Video, DAL.DTO.Video>().ReverseMap();
        CreateMap<AppUser, DAL.DTO.Identity.AppUser>().ReverseMap();
    }
}