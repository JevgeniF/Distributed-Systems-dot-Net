using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IUnitOfWork
{
    // cast
    ICastRoleRepository CastRole { get; }
    ICastInMovieRepository CastInMovie { get; }

    // common
    IPersonRepository Person { get; }

    // movie standard details
    IAgeRatingRepository AgeRating { get; }
    IGenreRepository Genre { get; }
    IMovieTypeRepository MovieType { get; }

    // movie
    IVideoRepository Video { get; }
    IUserRatingRepository UserRating { get; }
    IMovieGenreRepository MovieGenre { get; }
    IMovieDetailsRepository MovieDetails { get; }
    IMovieDbScoreRepository MovieDbScore { get; }

    // profile
    IUserProfileRepository UserProfile { get; }
    IProfileMovieRepository ProfileMovie { get; }
    IProfileFavoriteMovieRepository ProfileFavoriteMovie { get; }

    // user
    ISubscriptionRepository Subscription { get; }
    IUserSubscriptionRepository UserSubscription { get; }
    IPaymentDetailsRepository PaymentDetails { get; }
}