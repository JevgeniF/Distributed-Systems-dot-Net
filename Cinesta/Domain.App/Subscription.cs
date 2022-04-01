using Domain.Identity;

namespace Domain;

public class Subscription : BaseEntity
{
    public string Naming { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Price { get; set; } = default!;
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}