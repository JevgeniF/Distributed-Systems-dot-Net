using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class PaymentDetails : DomainEntityId
{
    [MaxLength(25)]
    [Display(ResourceType = typeof(Resources.App.Domain.User.PaymentDetails), Name = nameof(CardType))]
    public string CardType { get; set; } = default!;

    [MaxLength(16)]
    [Display(ResourceType = typeof(Resources.App.Domain.User.PaymentDetails), Name = nameof(CardNumber))]
    public string CardNumber { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.App.Domain.User.PaymentDetails), Name = nameof(ValidDate))]
    public DateTime ValidDate { get; set; }

    [MaxLength(3)]
    [Display(ResourceType = typeof(Resources.App.Domain.User.PaymentDetails), Name = nameof(SecurityCode))]
    public string SecurityCode { get; set; } = default!;

    public Guid AppUserId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.User.PaymentDetails), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}