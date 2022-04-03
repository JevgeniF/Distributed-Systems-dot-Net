using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.User;

public class PaymentDetails : DomainEntityMetaId
{
    [MaxLength(25)] public string CardType { get; set; } = default!;
    [MaxLength(16)] public string CardNumber { get; set; } = default!;
    public DateOnly ValidDate { get; set; }
    [MaxLength(3)] public string SecurityCode { get; set; } = default!;

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}