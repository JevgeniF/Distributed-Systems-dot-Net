﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class AppUser : IdentityUser<Guid>
{
   public Subscription? Subscription { get; set; }
}