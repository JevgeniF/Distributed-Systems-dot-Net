using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBll : IBll
{
    ICastRoleService CastRole { get; }
    ICastInMovieService CastInMovie { get; }
    IPersonService Person { get; }
    IAgeRatingService AgeRating { get; }
    IGenreService Genre { get; }
    IMovieTypeService MovieType { get; }
    IVideoService Video { get; }
    IMovieGenreService MovieGenre { get; }
    IMovieDetailsService MovieDetails { get; }
    IMovieDbScoreService MovieDbScore { get; }
    IUserProfileService UserProfile { get; }
    IProfileMovieService ProfileMovie { get; }
    IProfileFavoriteMovieService ProfileFavoriteMovie { get; }
    ISubscriptionService Subscription { get; }
    IUserSubscriptionService UserSubscription { get; }
    IPaymentDetailsService PaymentDetails { get; }
}