using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.DTO.Identity;
using Base.Domain;

namespace App.DTO.User;

public class Subscription: DomainEntityId<Guid>
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();

    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Description))]
    public LangStr Description { get; set; } = new();

    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Price))]
    public double Price { get; set; }

    public Guid AppUserId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}