using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    public Guid? PersonId { get; set; } = default!;
    [MinLength(1)] [MaxLength(50)] public string Name { get; set; } = default!;
    [MinLength(1)] [MaxLength(50)] public string Surname { get; set; } = default!;
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}