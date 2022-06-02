using App.DAL.DTO;
using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<AgeRating, Domain.AgeRating>().ReverseMap();
        CreateMap<CastInMovie, Domain.CastInMovie>().ReverseMap();
        CreateMap<CastRole, Domain.CastRole>().ReverseMap();
        CreateMap<Genre, Domain.Genre>().ReverseMap();
        CreateMap<MovieDbScore, Domain.MovieDbScore>().ReverseMap();
        CreateMap<MovieDetails, Domain.MovieDetails>().ReverseMap();
        CreateMap<MovieGenre, Domain.MovieGenre>().ReverseMap();
        CreateMap<MovieType, Domain.MovieType>().ReverseMap();
        CreateMap<PaymentDetails, Domain.PaymentDetails>().ReverseMap();
        CreateMap<Person, Domain.Person>().ReverseMap();
        CreateMap<ProfileFavoriteMovie, Domain.ProfileFavoriteMovie>().ReverseMap();
        CreateMap<ProfileMovie, Domain.ProfileMovie>().ReverseMap();
        CreateMap<Subscription, Domain.Subscription>().ReverseMap();
        CreateMap<UserProfile, Domain.UserProfile>().ReverseMap();
        CreateMap<UserSubscription, Domain.UserSubscription>().ReverseMap();
        CreateMap<Video, Domain.Video>().ReverseMap();
        CreateMap<AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}