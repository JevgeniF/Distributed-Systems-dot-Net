using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.User;

public class Subscription : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.Subscription), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.Subscription), Name = nameof(Description))]
    public LangStr Description { get; set; } = new();
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.Subscription), Name = nameof(Price))]
    public  double Price { get; set; }
    
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.Subscription), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}