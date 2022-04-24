using App.Contracts.DAL.Cast;
using App.Contracts.DAL.Common;
using App.Contracts.DAL.Movie;
using App.Contracts.DAL.MovieStandardDetails;
using App.Contracts.DAL.Profile;
using App.Contracts.DAL.User;
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
    IMovieDBScoreRepository MovieDbScore { get; }

    // profile
    IUserProfileRepository UserProfile { get; }
    IProfileMovieRepository ProfileMovie { get; }
    IProfileFavoriteMovieRepository ProfileFavoriteMovie { get; }

    // user
    ISubscriptionRepository Subscription { get; }
    IPaymentDetailsRepository PaymentDetails { get; }
}