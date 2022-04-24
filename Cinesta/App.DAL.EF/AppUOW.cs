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
    public AppUOW(AppDbContext dbContext) : base(dbContext)
    {
    }

    // cast
    private ICastRoleRepository? _castRole;
    public virtual ICastRoleRepository CastRole => _castRole ??= new CastRoleRepository(UOWDbContext);
    private ICastInMovieRepository? _castInMovie;
    public virtual ICastInMovieRepository CastInMovie => _castInMovie ??= new CastInMovieRepository(UOWDbContext);

    // person
    private IPersonRepository? _person;
    public virtual IPersonRepository Person => _person ??= new PersonRepository(UOWDbContext);

    // movie standard details
    private IAgeRatingRepository? _ageRating;
    public virtual IAgeRatingRepository AgeRating => _ageRating ??= new AgeRatingRepository(UOWDbContext);
    private IGenreRepository? _genre;
    public virtual IGenreRepository Genre => _genre ??= new GenreRepository(UOWDbContext);
    private IMovieTypeRepository? _movieType;
    public virtual IMovieTypeRepository MovieType => _movieType ??= new MovieTypeRepository(UOWDbContext);
    
    // movie
    private IVideoRepository? _video;
    public IVideoRepository Video => _video ??= new VideoRepository(UOWDbContext);
    private IUserRatingRepository? _userRating;
    public virtual IUserRatingRepository UserRating => _userRating ??= new UserRatingRepository(UOWDbContext);
    private IMovieGenreRepository? _movieGenre;
    public virtual IMovieGenreRepository MovieGenre => _movieGenre ??= new MovieGenreRepository(UOWDbContext);
    private IMovieDetailsRepository? _movieDetails;
    public virtual IMovieDetailsRepository MovieDetails => _movieDetails ??= new MovieDetailsRepository(UOWDbContext);
    private IMovieDBScoreRepository? _movieDbScore;
    public virtual IMovieDBScoreRepository MovieDbScore => _movieDbScore ??= new MovieDBScoreRepository(UOWDbContext);

    // profile
    private IUserProfileRepository? _userProfile;
    public virtual IUserProfileRepository UserProfile => _userProfile ??= new UserProfileRepository(UOWDbContext);
    private IProfileMovieRepository? _profileMovie;
    public virtual IProfileMovieRepository ProfileMovie => _profileMovie ??= new ProfileMovieRepository(UOWDbContext);
    private IProfileFavoriteMovieRepository? _profileFavoriteMovie;
    public virtual IProfileFavoriteMovieRepository ProfileFavoriteMovie => _profileFavoriteMovie ??=
        new ProfileFavoriteMovieRepository(UOWDbContext);

    // user
    private ISubscriptionRepository? _subscription;
    public virtual ISubscriptionRepository Subscription => _subscription ??= new SubscriptionRepository(UOWDbContext);
    private IPaymentDetailsRepository? _paymentDetails;
    public virtual IPaymentDetailsRepository PaymentDetails => _paymentDetails ??=
        new PaymentDetailsRepository(UOWDbContext);
}