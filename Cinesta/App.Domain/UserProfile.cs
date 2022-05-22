using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class UserProfile : DomainEntityMetaId
{
    [MaxLength(150)] public string IconUri { get; set; } = default!;

    [MaxLength(50)] public string Name { get; set; } = default!;

    public int Age { get; set; }

    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }

   // public ICollection<ProfileMovie>? ProfileMovies { get; set; }
   // public ICollection<ProfileFavoriteMovie>? ProfileFavoriteMovies { get; set; }
}