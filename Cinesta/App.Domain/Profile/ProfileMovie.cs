using System.ComponentModel.DataAnnotations;
using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Profile;

public class ProfileMovie : DomainEntityMetaId
{
    public Guid UserProfileId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileMovie), Name = nameof(UserProfile))]
    public UserProfile? UserProfile { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileMovie), Name = nameof(UserProfile))]
    public MovieDetails? MovieDetails { get; set; }
}