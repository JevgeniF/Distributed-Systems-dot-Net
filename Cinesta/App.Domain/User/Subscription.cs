using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.User;

public class Subscription : DomainEntityMetaId
{
    [MaxLength(25)]
    public string Naming { get; set; } = default!;
    [MaxLength(250)]
    public string Description { get; set; } = default!;
    public  double Price { get; set; } = default!;
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}