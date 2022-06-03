using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    
    [MinLength(1)] [MaxLength(50)] public string Name { get; set; } = default!;
    [MinLength(1)] [MaxLength(50)] public string Surname { get; set; } = default!;
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
    
    public Guid? PersonId { get; set; } = default!;
    public Person? Person { get; set; }
    public ICollection<ApartRent>? Rents { get; set; }
}