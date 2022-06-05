using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class UserProfile : DomainEntityId
{
    [MaxLength(150)]
    [Display(ResourceType = typeof(Resources.App.Domain.Profile.UserProfile), Name = nameof(IconUri))]
    public string IconUri { get; set; } = default!;

    [MaxLength(50)]
    [Display(ResourceType = typeof(Resources.App.Domain.Profile.UserProfile), Name = nameof(Name))]
    public string Name { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.UserProfile), Name = nameof(Age))]
    public int Age { get; set; }

    public Guid AppUserId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.UserProfile), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}