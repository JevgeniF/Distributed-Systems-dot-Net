using App.Public.DTO.v1;
using AutoMapper;

namespace App.Public;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<AgeRating, BLL.DTO.AgeRating>().ReverseMap();
        CreateMap<CastInMovie, BLL.DTO.CastInMovie>().ReverseMap();
        CreateMap<CastRole, BLL.DTO.CastRole>().ReverseMap();
        CreateMap<Genre, BLL.DTO.Genre>().ReverseMap();
        CreateMap<MovieDbScore, BLL.DTO.MovieDbScore>().ReverseMap();
        CreateMap<MovieDetails, BLL.DTO.MovieDetails>().ReverseMap();
        CreateMap<MovieGenre, BLL.DTO.MovieGenre>().ReverseMap();
        CreateMap<MovieType, BLL.DTO.MovieType>().ReverseMap();
        CreateMap<PaymentDetails, BLL.DTO.PaymentDetails>().ReverseMap();
        CreateMap<Person, BLL.DTO.Person>().ReverseMap();
        CreateMap<ProfileFavoriteMovie, BLL.DTO.ProfileFavoriteMovie>().ReverseMap();
        CreateMap<ProfileMovie, BLL.DTO.ProfileMovie>().ReverseMap();
        CreateMap<Subscription, BLL.DTO.Subscription>().ReverseMap();
        CreateMap<UserProfile, BLL.DTO.UserProfile>().ReverseMap();
        CreateMap<UserRating, BLL.DTO.UserRating>().ReverseMap();
        CreateMap<UserSubscription, BLL.DTO.UserSubscription>().ReverseMap();
        CreateMap<Video, BLL.DTO.Video>().ReverseMap();
        //CreateMap<AppUser, BLL.DTO.Identity.AppUser>().ReverseMap();
    }
}