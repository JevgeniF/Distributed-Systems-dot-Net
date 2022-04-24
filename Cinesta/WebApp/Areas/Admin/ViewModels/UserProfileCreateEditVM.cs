using App.Domain.Profile;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class UserProfileCreateEditVM
{
    public UserProfile UserProfile { get; set; } = default!;

    public SelectList? AppUserSelectList { get; set; }
}