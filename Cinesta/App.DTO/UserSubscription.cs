using App.DTO.Identity;
using Base.Domain;

namespace App.DTO;

public class UserSubscription: DomainEntityId
{
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
}
