using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Profile;

public class ProfileFavoriteMovies : DomainEntityMetaId
{
    public Guid UserProfileId { get; set;}
    public UserProfile? UserProfile { get; set; }
    
    public ICollection<MovieDetails>? FavoriteMovieDetailsCollection { get; set; }
}