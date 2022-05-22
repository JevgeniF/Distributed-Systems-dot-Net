using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class UserSubscription : DomainEntityId
{
    public Guid? AppUserId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.UserSubscription), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

    public Guid? SubscriptionId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.UserSubscription), Name = nameof(Subscription))]
    public Subscription? Subscription { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.UserSubscription), Name = nameof(ExpirationDateTime))]
    [DataType(DataType.Date)]
    public DateTime ExpirationDateTime { get; set; } = DateTime.UtcNow.AddMonths(1);
}