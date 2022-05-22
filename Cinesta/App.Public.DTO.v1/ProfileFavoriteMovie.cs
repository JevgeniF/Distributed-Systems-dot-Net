using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class ProfileFavoriteMovie : DomainEntityId
{
    public Guid UserProfileId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileFavoriteMovie), Name = nameof(UserProfile))]
    public UserProfile? UserProfile { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileFavoriteMovie), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}