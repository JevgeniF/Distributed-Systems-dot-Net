using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Subscription : DomainEntityId
{
    [MaxLength(100)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();

    [MaxLength(250)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Description))]
    public LangStr Description { get; set; } = new();

    public int ProfilesCount { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Price))]
    public double Price { get; set; }
}