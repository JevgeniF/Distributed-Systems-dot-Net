using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBll : BaseBll<IAppUOW>, IAppBll
{
    private readonly IMapper _mapper;

    private IAgeRatingService? _ageRating;
    private ICastInMovieService? _castInMovie;
    private ICastRoleService? _castRole;
    private IGenreService? _genre;
    private IMovieDbScoreService? _movieDbScore;
    private IMovieDetailsService? _movieDetails;
    private IMovieGenreService? _movieGenre;
    private IMovieTypeService? _movieType;
    private IPaymentDetailsService? _paymentDetails;
    private IPersonService? _person;
    private IProfileFavoriteMovieService? _profileFavoriteMovie;
    private IProfileMovieService? _profileMovie;
    private ISubscriptionService? _subscription;
    private IUserProfileService? _userProfile;
    private IUserSubscriptionService? _userSubscription;
    private IVideoService? _video;
    protected IAppUOW UOW;

    public AppBll(IAppUOW uow, IMapper mapper)
    {
        UOW = uow;
        _mapper = mapper;
    }

    public override async Task<int> SaveChangesAsync()
    {
        return await UOW.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return UOW.SaveChanges();
    }

    public virtual ICastRoleService CastRole =>
        _castRole ??= new CastRoleService(UOW.CastRole, new CastRoleMapper(_mapper));

    public virtual ICastInMovieService CastInMovie =>
        _castInMovie ??= new CastInMovieService(UOW.CastInMovie, new CastInMovieMapper(_mapper));

    public virtual IPersonService Person =>
        _person ??= new PersonService(UOW.Person, new PersonMapper(_mapper));

    public virtual IAgeRatingService AgeRating =>
        _ageRating ??= new AgeRatingService(UOW.AgeRating, new AgeRatingMapper(_mapper));

    public virtual IGenreService Genre => _genre ??= new GenreService(UOW.Genre, new GenreMapper(_mapper));

    public virtual IMovieTypeService MovieType =>
        _movieType ??= new MovieTypeService(UOW.MovieType, new MovieTypeMapper(_mapper));

    public IVideoService Video => _video ??= new VideoService(UOW.Video, new VideoMapper(_mapper));
    
    public virtual IMovieGenreService MovieGenre =>
        _movieGenre ??= new MovieGenreService(UOW.MovieGenre, new MovieGenreMapper(_mapper));

    public virtual IMovieDetailsService MovieDetails =>
        _movieDetails ??= new MovieDetailsService(UOW.MovieDetails, new MovieDetailsMapper(_mapper));

    public virtual IMovieDbScoreService MovieDbScore =>
        _movieDbScore ??= new MovieDbScoreService(UOW.MovieDbScore, new MovieDbScoreMapper(_mapper));

    public virtual IUserProfileService UserProfile =>
        _userProfile ??= new UserProfileService(UOW.UserProfile, new UserProfileMapper(_mapper));

    public virtual IProfileMovieService ProfileMovie =>
        _profileMovie ??= new ProfileMovieService(UOW.ProfileMovie, new ProfileMovieMapper(_mapper));

    public virtual IProfileFavoriteMovieService ProfileFavoriteMovie =>
        _profileFavoriteMovie ??=
            new ProfileFavoriteMovieService(UOW.ProfileFavoriteMovie, new ProfileFavoriteMovieMapper(_mapper));

    public virtual ISubscriptionService Subscription =>
        _subscription ??= new SubscriptionService(UOW.Subscription, new SubscriptionMapper(_mapper));

    public virtual IUserSubscriptionService UserSubscription =>
        _userSubscription ??= new UserSubscriptionService(UOW.UserSubscription, new UserSubscriptionMapper(_mapper));

    public virtual IPaymentDetailsService PaymentDetails => _paymentDetails ??=
        new PaymentDetailsService(UOW.PaymentDetails, new PaymentDetailsMapper(_mapper));
}