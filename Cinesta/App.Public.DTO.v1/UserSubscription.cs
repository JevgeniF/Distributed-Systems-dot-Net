
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class UserSubscription : DomainEntityId
{
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
}