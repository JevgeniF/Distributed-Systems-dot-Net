﻿using App.Domain.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class SubscriptionCreateEditVM
{
    public Subscription Subscription { get; set; } = default!;
    
    public SelectList? AppUserSelectList { get; set; }
}