using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class Subscription : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();

    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Description))]
    public LangStr Description { get; set; } = new();

    public int ProfilesCount { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(Price))]
    public double Price { get; set; }

    public Guid AppUserId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.Subscription), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}