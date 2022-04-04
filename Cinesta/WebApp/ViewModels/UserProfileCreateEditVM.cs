﻿using App.Domain.Profile;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class UserProfileCreateEditVM
{
    public UserProfile UserProfile { get; set; } = default!;
    
    public SelectList? AppUserSelectList { get; set; }
}