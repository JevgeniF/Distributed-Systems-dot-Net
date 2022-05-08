using System.ComponentModel.DataAnnotations;
using App.DTO.Common;
using App.DTO.Profile;
using Base.Domain;

namespace App.DTO.Identity;

public class AppUser: DomainEntityId<Guid>
{
    public Guid? PersonId { get; set; } = default!;

    [MinLength(1)] [MaxLength(25)] public string Name { get; set; } = default!;
    [MinLength(1)] [MaxLength(25)] public string Surname { get; set; } = default!;

    public Person? Person { get; set; }
    
    public ICollection<UserProfile>? UserProfiles { get; set; }
}