using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Subscription : DomainEntityId
{
    [MaxLength(100)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Naming))]
    public string Naming { get; set; } = default!;

    [MaxLength(250)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Description))]
    public string Description { get; set; } = default!;

    public int ProfilesCount { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Price))]
    public double Price { get; set; }
}