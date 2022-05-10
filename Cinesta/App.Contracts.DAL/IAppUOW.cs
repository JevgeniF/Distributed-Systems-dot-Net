using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IUnitOfWork
{
   
    ICastRoleRepository CastRole { get; }
    ICastInMovieRepository CastInMovie { get; }
    IPersonRepository Person { get; }
    IAgeRatingRepository AgeRating { get; }
    IGenreRepository Genre { get; }
    IMovieTypeRepository MovieType { get; }
    IVideoRepository Video { get; }
    IUserRatingRepository UserRating { get; }
    IMovieGenreRepository MovieGenre { get; }
    IMovieDetailsRepository MovieDetails { get; }
    IMovieDbScoreRepository MovieDbScore { get; }
    IUserProfileRepository UserProfile { get; }
    IProfileMovieRepository ProfileMovie { get; }
    IProfileFavoriteMovieRepository ProfileFavoriteMovie { get; }
    ISubscriptionRepository Subscription { get; }
    IUserSubscriptionRepository UserSubscription { get; }
    IPaymentDetailsRepository PaymentDetails { get; }
}