using App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class UserSubscriptionCreateVM
{
    public UserSubscription UserSubscription { get; set; } = default!;
    public SelectList? SubscriptionSelectList { get; set; }
}