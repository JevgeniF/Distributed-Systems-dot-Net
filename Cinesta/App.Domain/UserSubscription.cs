using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class UserSubscription : DomainEntityMetaId
{
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }

    [DataType(DataType.Date)] public DateTime ExpirationDateTime { get; set; }
}