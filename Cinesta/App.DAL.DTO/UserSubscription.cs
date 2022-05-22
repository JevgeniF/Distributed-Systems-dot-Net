using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Domain;

namespace App.DAL.DTO;

public class UserSubscription : DomainEntityId
{
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    [DataType(DataType.Date)] public DateTime ExpirationDateTime { get; set; }
}