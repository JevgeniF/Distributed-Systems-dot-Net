using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class UserProfile : DomainEntityId
{
    [MaxLength(150)] public string IconUri { get; set; } = default!;

    [MaxLength(50)] public string Name { get; set; } = default!;

    public int Age { get; set; }

    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }

    public ICollection<ProfileMovie>? ProfileMovies { get; set; }
    public ICollection<ProfileFavoriteMovie>? ProfileFavoriteMovies { get; set; }
}