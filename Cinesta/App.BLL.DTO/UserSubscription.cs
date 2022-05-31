using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class UserSubscription : DomainEntityId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    public DateTime ExpirationDateTime { get; set; }
}