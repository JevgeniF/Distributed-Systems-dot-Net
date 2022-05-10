using App.BLL.DTO.Identity;
using Base.Domain;

namespace WebApp.DTO;

public class SubscriptionDto : DomainEntityId
{
    public string Naming { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Price { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}