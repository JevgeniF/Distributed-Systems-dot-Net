using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.User;

public class PaymentDetails : DomainEntityMetaId
{
    [MaxLength(25)] 
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.PaymentDetails), Name = nameof(CardType))]
    public string CardType { get; set; } = default!;
    [MaxLength(16)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.PaymentDetails), Name = nameof(CardNumber))]
    public string CardNumber { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.PaymentDetails), Name = nameof(ValidDate))]
    public DateTime ValidDate { get; set; }
    [MaxLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.PaymentDetails), Name = nameof(SecurityCode))]
    public string SecurityCode { get; set; } = default!;

    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.User.PaymentDetails), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}