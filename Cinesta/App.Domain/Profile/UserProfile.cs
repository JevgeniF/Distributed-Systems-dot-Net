using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Profile;

public class UserProfile : DomainEntityMetaId
{
    [MaxLength (100)]
    public string IconUri { get; set; } = default!;
    [MaxLength (25)]
    public string Name { get; set; } = default!;
    public int Age { get; set; }
}