using System.ComponentModel.DataAnnotations;
using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Profile;

public class ProfileFavoriteMovie : DomainEntityMetaId
{
    public Guid UserProfileId { get; set;}
    [Display(ResourceType = typeof(App.Resources.App.Domain.Profile.ProfileFavoriteMovie), Name = nameof(UserProfile))]
    public UserProfile? UserProfile { get; set; }
    
    public Guid MovieDetailsId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Profile.ProfileFavoriteMovie), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}