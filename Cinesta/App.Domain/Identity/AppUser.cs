using System.ComponentModel.DataAnnotations;
using App.Domain.Common;
using App.Domain.Profile;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    public Guid? PersonId { get; set; } = default!;

    [MinLength(1)] [MaxLength(25)] public string Name { get; set; } = default!;
    [MinLength(1)] [MaxLength(25)] public string Surname { get; set; } = default!;

    public Person? Person { get; set; }

    public ICollection<RefreshToken>? RefreshTokens { get; set; }
    public ICollection<UserProfile>? UserProfiles { get; set; }
}