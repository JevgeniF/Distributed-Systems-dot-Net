using App.Domain.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class PaymentDetailsCreateEditVM
{
    public PaymentDetails PaymentDetails { get; set; } = default!;
    
    public SelectList? AppUserSelectList { get; set; }
}