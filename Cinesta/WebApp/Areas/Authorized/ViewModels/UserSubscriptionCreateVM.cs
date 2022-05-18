#pragma warning disable CS1591
using Microsoft.AspNetCore.Mvc.Rendering;
using UserSubscription = App.Domain.UserSubscription;

namespace WebApp.Areas.Authorized.ViewModels;

public class UserSubscriptionCreateVM
{
    public UserSubscription UserSubscription { get; set; } = default!;
    public SelectList? SubscriptionSelectList { get; set; }
}