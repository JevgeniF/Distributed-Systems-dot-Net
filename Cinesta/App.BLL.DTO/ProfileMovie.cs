using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class ProfileMovie: DomainEntityId
{
    public Guid UserProfileId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileMovie), Name = nameof(UserProfile))]
    public UserProfile? UserProfile { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileMovie), Name = nameof(UserProfile))]
    public MovieDetails? MovieDetails { get; set; }
}