using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.User;

public class UserSubscription: DomainEntityMetaId<Guid>
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
}