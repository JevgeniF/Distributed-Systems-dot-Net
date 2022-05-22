using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class PaymentDetails : DomainEntityMetaId
{
    [MaxLength(25)]
    public string CardType { get; set; } = default!;

    [MinLength(16)][MaxLength(16)]
    public string CardNumber { get; set; } = default!;
    
    [DataType(DataType.Date)] public DateTime ValidDate { get; set; }

    [MinLength(3)][MaxLength(3)]
    public string SecurityCode { get; set; } = default!;

    public Guid AppUserId { get; set; }
    
    public AppUser? AppUser { get; set; }
}