using App.Contracts.DAL;
using App.Contracts.DAL.Cast;
using App.Contracts.DAL.Common;
using App.Contracts.DAL.Movie;
using App.Contracts.DAL.MovieStandardDetails;
using App.Contracts.DAL.Profile;
using App.Contracts.DAL.User;
using App.DAL.EF.Repositories.Cast;
using App.DAL.EF.Repositories.Common;
using App.DAL.EF.Repositories.Movie;
using App.DAL.EF.Repositories.MovieStandardDetails;
using App.DAL.EF.Repositories.Profile;
using App.DAL.EF.Repositories.User;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUOW
{
    private IAgeRatingRepository? _ageRating;
    private ICastInMovieRepository? _castInMovie;
    private ICastRoleRepository? _castRole;
    private IGenreRepository? _genre;
    private IMovieDBScoreRepository? _movieDbScore;
    private IMovieDetailsRepository? _movieDetails;
    private IMovieGenreRepository? _movieGenre;
    private IMovieTypeRepository? _movieType;
    private IPaymentDetailsRepository? _paymentDetails;
    private IPersonRepository? _person;
    private IProfileFavoriteMovieRepository? _profileFavoriteMovie;
    private IProfileMovieRepository? _profileMovie;
    private ISubscriptionRepository? _subscription;
    private IUserProfileRepository? _userProfile;
    private IUserSubscriptionRepository? _userSubscription;
    private IUserRatingRepository? _userRating;
    private IVideoRepository? _video;

    public AppUOW(AppDbContext dbContext) : base(dbContext)
    {
    }

    public virtual ICastRoleRepository CastRole => _castRole ??= new CastRoleRepository(UOWDbContext);
    public virtual ICastInMovieRepository CastInMovie => _castInMovie ??= new CastInMovieRepository(UOWDbContext);
    public virtual IPersonRepository Person => _person ??= new PersonRepository(UOWDbContext);
    public virtual IAgeRatingRepository AgeRating => _ageRating ??= new AgeRatingRepository(UOWDbContext);
    public virtual IGenreRepository Genre => _genre ??= new GenreRepository(UOWDbContext);
    public virtual IMovieTypeRepository MovieType => _movieType ??= new MovieTypeRepository(UOWDbContext);
    public IVideoRepository Video => _video ??= new VideoRepository(UOWDbContext);
    public virtual IUserRatingRepository UserRating => _userRating ??= new UserRatingRepository(UOWDbContext);
    public virtual IMovieGenreRepository MovieGenre => _movieGenre ??= new MovieGenreRepository(UOWDbContext);
    public virtual IMovieDetailsRepository MovieDetails => _movieDetails ??= new MovieDetailsRepository(UOWDbContext);
    public virtual IMovieDBScoreRepository MovieDbScore => _movieDbScore ??= new MovieDBScoreRepository(UOWDbContext);
    public virtual IUserProfileRepository UserProfile => _userProfile ??= new UserProfileRepository(UOWDbContext);
    public virtual IProfileMovieRepository ProfileMovie => _profileMovie ??= new ProfileMovieRepository(UOWDbContext);

    public virtual IProfileFavoriteMovieRepository ProfileFavoriteMovie => _profileFavoriteMovie ??=
        new ProfileFavoriteMovieRepository(UOWDbContext);

    public virtual ISubscriptionRepository Subscription => _subscription ??= new SubscriptionRepository(UOWDbContext);

    public virtual IUserSubscriptionRepository UserSubscription =>
        _userSubscription ??= new UserSubscriptionRepository(UOWDbContext);

    public virtual IPaymentDetailsRepository PaymentDetails => _paymentDetails ??=
        new PaymentDetailsRepository(UOWDbContext);
}