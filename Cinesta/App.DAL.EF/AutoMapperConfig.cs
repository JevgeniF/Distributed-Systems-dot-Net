using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<DTO.AgeRating, Domain.AgeRating>().ReverseMap();
        CreateMap<DTO.CastInMovie, Domain.CastInMovie>().ReverseMap();
        CreateMap<DTO.CastRole, Domain.CastRole>().ReverseMap();
        CreateMap<DTO.Genre, Domain.Genre>().ReverseMap();
        CreateMap<DTO.MovieDbScore, Domain.MovieDbScore>().ReverseMap();
        CreateMap<DTO.MovieDetails, Domain.MovieDetails>().ReverseMap();
        CreateMap<DTO.MovieGenre, Domain.MovieGenre>().ReverseMap();
        CreateMap<DTO.MovieType, Domain.MovieType>().ReverseMap();
        CreateMap<DTO.PaymentDetails, Domain.PaymentDetails>().ReverseMap();
        CreateMap<DTO.Person, Domain.Person>().ReverseMap();
        CreateMap<DTO.ProfileFavoriteMovie, Domain.ProfileFavoriteMovie>().ReverseMap();
        CreateMap<DTO.ProfileMovie, Domain.ProfileMovie>().ReverseMap();
        CreateMap<DTO.Subscription, Domain.Subscription>().ReverseMap();
        CreateMap<DTO.UserProfile, Domain.UserProfile>().ReverseMap();
        CreateMap<DTO.UserRating, Domain.UserRating>().ReverseMap();
        CreateMap<DTO.UserSubscription, Domain.UserSubscription>().ReverseMap();
        CreateMap<DTO.Video, Domain.Video>().ReverseMap();
        CreateMap<DTO.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}