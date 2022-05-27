#pragma warning disable CS1591
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class UserSubscriptionCreateVm
{
    public UserSubscription UserSubscription { get; set; } = default!;
    public SelectList? SubscriptionSelectList { get; set; }
}