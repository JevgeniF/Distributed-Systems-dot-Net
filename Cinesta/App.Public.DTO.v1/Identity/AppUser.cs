using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Base.Domain;

namespace App.Public.DTO.v1.Identity;

public class AppUser : DomainEntityId
{
    [JsonIgnore]
    public Guid? PersonId { get; set; } = default!;

    [MinLength(1)] [MaxLength(50)] public string Name { get; set; } = default!;
    [MinLength(1)] [MaxLength(50)] public string Surname { get; set; } = default!;

    [JsonIgnore]
    public Person? Person { get; set; }

    [JsonIgnore]
    public ICollection<UserProfile>? UserProfiles { get; set; }
}