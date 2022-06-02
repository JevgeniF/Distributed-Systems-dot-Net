using Base.Domain;

namespace App.Domain;

public class ProfileFavoriteMovie : DomainEntityMetaId
{
    public Guid UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}