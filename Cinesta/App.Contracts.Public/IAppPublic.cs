using App.Contracts.Public.Models;
using Base.Contracts.Public;

namespace App.Contracts.Public;

public interface IAppPublic : IPublic
{
    ICastRoleModel CastRole { get; }
    ICastInMovieModel CastInMovie { get; }
    IPersonModel Person { get; }
    IAgeRatingModel AgeRating { get; }
    IGenreModel Genre { get; }
    IMovieTypeModel MovieType { get; }
    IVideoModel Video { get; }
    IMovieGenreModel MovieGenre { get; }
    IMovieDetailsModel MovieDetails { get; }
    IMovieDbScoreModel MovieDbScore { get; }
    IUserProfileModel UserProfile { get; }
    IProfileMovieModel ProfileMovie { get; }
    IProfileFavoriteMovieModel ProfileFavoriteMovie { get; }
    ISubscriptionModel Subscription { get; }
    IUserSubscriptionModel UserSubscription { get; }
    IPaymentDetailsModel PaymentDetails { get; }
}