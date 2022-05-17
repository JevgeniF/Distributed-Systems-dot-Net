using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUOW
{
    private readonly IMapper _mapper;

    private IAgeRatingRepository? _ageRating;
    private ICastInMovieRepository? _castInMovie;
    private ICastRoleRepository? _castRole;
    private IGenreRepository? _genre;
    private IMovieDbScoreRepository? _movieDbScore;
    private IMovieDetailsRepository? _movieDetails;
    private IMovieGenreRepository? _movieGenre;
    private IMovieTypeRepository? _movieType;
    private IPaymentDetailsRepository? _paymentDetails;
    private IPersonRepository? _person;
    private IProfileFavoriteMovieRepository? _profileFavoriteMovie;
    private IProfileMovieRepository? _profileMovie;
    private ISubscriptionRepository? _subscription;
    private IUserProfileRepository? _userProfile;
    private IUserRatingRepository? _userRating;
    private IUserSubscriptionRepository? _userSubscription;
    private IVideoRepository? _video;

    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public virtual ICastRoleRepository CastRole =>
        _castRole ??= new CastRoleRepository(UOWDbContext, new CastRoleMapper(_mapper));

    public virtual ICastInMovieRepository CastInMovie =>
        _castInMovie ??= new CastInMovieRepository(UOWDbContext, new CastInMovieMapper(_mapper));

    public virtual IPersonRepository Person =>
        _person ??= new PersonRepository(UOWDbContext, new PersonMapper(_mapper));

    public virtual IAgeRatingRepository AgeRating =>
        _ageRating ??= new AgeRatingRepository(UOWDbContext, new AgeRatingMapper(_mapper));

    public virtual IGenreRepository Genre => _genre ??= new GenreRepository(UOWDbContext, new GenreMapper(_mapper));

    public virtual IMovieTypeRepository MovieType =>
        _movieType ??= new MovieTypeRepository(UOWDbContext, new MovieTypeMapper(_mapper));

    public IVideoRepository Video => _video ??= new VideoRepository(UOWDbContext, new VideoMapper(_mapper));

    public virtual IUserRatingRepository UserRating =>
        _userRating ??= new UserRatingRepository(UOWDbContext, new UserRatingMapper(_mapper));

    public virtual IMovieGenreRepository MovieGenre =>
        _movieGenre ??= new MovieGenreRepository(UOWDbContext, new MovieGenreMapper(_mapper));

    public virtual IMovieDetailsRepository MovieDetails =>
        _movieDetails ??= new MovieDetailsRepository(UOWDbContext, new MovieDetailsMapper(_mapper));

    public virtual IMovieDbScoreRepository MovieDbScore =>
        _movieDbScore ??= new MovieDbScoreRepository(UOWDbContext, new MovieDbScoreMapper(_mapper));

    public virtual IUserProfileRepository UserProfile =>
        _userProfile ??= new UserProfileRepository(UOWDbContext, new UserProfileMapper(_mapper));

    public virtual IProfileMovieRepository ProfileMovie =>
        _profileMovie ??= new ProfileMovieRepository(UOWDbContext, new ProfileMovieMapper(_mapper));

    public virtual IProfileFavoriteMovieRepository ProfileFavoriteMovie => _profileFavoriteMovie ??=
        new ProfileFavoriteMovieRepository(UOWDbContext, new ProfileFavoriteMovieMapper(_mapper));

    public virtual ISubscriptionRepository Subscription =>
        _subscription ??= new SubscriptionRepository(UOWDbContext, new SubscriptionMapper(_mapper));

    public virtual IUserSubscriptionRepository UserSubscription =>
        _userSubscription ??= new UserSubscriptionRepository(UOWDbContext, new UserSubscriptionMapper(_mapper));

    public virtual IPaymentDetailsRepository PaymentDetails => _paymentDetails ??=
        new PaymentDetailsRepository(UOWDbContext, new PaymentDetailsMapper(_mapper));
}