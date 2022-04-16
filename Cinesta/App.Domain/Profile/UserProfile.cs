using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.Profile;

public class UserProfile : DomainEntityMetaId
{
    [MaxLength (100)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Profile.UserProfile), Name = nameof(IconUri))]
    public string IconUri { get; set; } = default!;
    [MaxLength (25)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Profile.UserProfile), Name = nameof(Name))]
    public string Name { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.App.Domain.Profile.UserProfile), Name = nameof(Age))]
    public int Age { get; set; }
    
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Profile.UserProfile), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
    public ICollection<ProfileMovie>? ProfileMovies { get; set; }
    public ICollection<ProfileFavoriteMovie>? ProfileFavoriteMovies { get; set; }
}