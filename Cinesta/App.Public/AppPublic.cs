using App.Contracts.BLL;
using App.Contracts.Public;
using App.Contracts.Public.Models;
using App.Public.Mappers;
using App.Public.Models;
using AutoMapper;
using Base.Public;

namespace App.Public;

public class AppPublic : BasePublic<IAppBll>, IAppPublic
{
    private readonly IMapper _mapper;

    private IAgeRatingModel? _ageRating;
    private ICastInMovieModel? _castInMovie;
    private ICastRoleModel? _castRole;
    private IGenreModel? _genre;
    private IMovieDbScoreModel? _movieDbScore;
    private IMovieDetailsModel? _movieDetails;
    private IMovieGenreModel? _movieGenre;
    private IMovieTypeModel? _movieType;
    private IPaymentDetailsModel? _paymentDetails;
    private IPersonModel? _person;
    private IProfileFavoriteMovieModel? _profileFavoriteMovie;
    private IProfileMovieModel? _profileMovie;
    private ISubscriptionModel? _subscription;
    private IUserProfileModel? _userProfile;
    private IUserSubscriptionModel? _userSubscription;
    private IVideoModel? _video;
    protected IAppBll Bll;

    public AppPublic(IAppBll bll, IMapper mapper)
    {
        Bll = bll;
        _mapper = mapper;
    }

    public override async Task<int> SaveChangesAsync()
    {
        return await Bll.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return Bll.SaveChanges();
    }

    public virtual ICastRoleModel CastRole =>
        _castRole ??= new CastRoleModel(Bll.CastRole, new CastRoleMapper(_mapper));

    public virtual ICastInMovieModel CastInMovie =>
        _castInMovie ??= new CastInMovieModel(Bll.CastInMovie, new CastInMovieMapper(_mapper));

    public virtual IPersonModel Person => _person ??= new PersonModel(Bll.Person, new PersonMapper(_mapper));

    public virtual IAgeRatingModel AgeRating =>
        _ageRating ??= new AgeRatingModel(Bll.AgeRating, new AgeRatingMapper(_mapper));

    public virtual IGenreModel Genre => _genre ??= new GenreModel(Bll.Genre, new GenreMapper(_mapper));

    public virtual IMovieTypeModel MovieType =>
        _movieType ??= new MovieTypeModel(Bll.MovieType, new MovieTypeMapper(_mapper));

    public virtual IVideoModel Video => _video ??= new VideoModel(Bll.Video, new VideoMapper(_mapper));
    
    public virtual IMovieGenreModel MovieGenre =>
        _movieGenre ??= new MovieGenreModel(Bll.MovieGenre, new MovieGenreMapper(_mapper));

    public virtual IMovieDetailsModel MovieDetails =>
        _movieDetails ??= new MovieDetailsModel(Bll.MovieDetails, new MovieDetailsMapper(_mapper));

    public virtual IMovieDbScoreModel MovieDbScore =>
        _movieDbScore ??= new MovieDbScoreModel(Bll.MovieDbScore, new MovieDbScoreMapper(_mapper));

    public virtual IUserProfileModel UserProfile =>
        _userProfile ??= new UserProfileModel(Bll.UserProfile, new UserProfileMapper(_mapper));

    public virtual IProfileMovieModel ProfileMovie =>
        _profileMovie ??= new ProfileMovieModel(Bll.ProfileMovie, new ProfileMovieMapper(_mapper));

    public virtual IProfileFavoriteMovieModel ProfileFavoriteMovie => _profileFavoriteMovie ??=
        new ProfileFavoriteMovieModel(Bll.ProfileFavoriteMovie, new ProfileFavoriteMovieMapper(_mapper));

    public virtual ISubscriptionModel Subscription =>
        _subscription ??= new SubscriptionModel(Bll.Subscription, new SubscriptionMapper(_mapper));

    public virtual IUserSubscriptionModel UserSubscription => _userSubscription ??=
        new UserSubscriptionModel(Bll.UserSubscription, new UserSubscriptionMapper(_mapper));

    public virtual IPaymentDetailsModel PaymentDetails => _paymentDetails ??=
        new PaymentDetailsModel(Bll.PaymentDetails, new PaymentDetailsMapper(_mapper));
}