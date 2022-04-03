using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Profile;

public class MoviesInProfileLibrary : DomainEntityMetaId
{
    public Guid UserProfileId { get; set;}
    public UserProfile? UserProfile { get; set; }
    
    public ICollection<MovieDetails>? MovieDetailsCollection { get; set; }
}