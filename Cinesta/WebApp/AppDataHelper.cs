﻿using App.DAL.EF;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public static class AppDataHelper
{
    public static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        if (context == null) throw new ApplicationException("Services error. No DB context");

        //TODO - check  database state
        //can't connect - wrong address
        //can't connect - wrong user/pass
        //can connect - no database
        //can connect - is database

        if (config.GetValue<bool>("DataInitialization:DropDatabase")) context.Database.EnsureDeleted();

        if (config.GetValue<bool>("DataInitialization:MigrateDatabase")) context.Database.Migrate();

        if (config.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

            if (userManager == null || roleManager == null)
                throw new NullReferenceException(
                    "userManager or roleManager cannot be null!");

            var roles = new[]
            {
                "admin",
                "user"
            };

            foreach (var roleInfo in roles)
            {
                var role = roleManager.FindByNameAsync(roleInfo).Result;
                if (role == null)
                {
                    var identityResult = roleManager.CreateAsync(new AppRole {Name = roleInfo}).Result;
                    if (!identityResult.Succeeded) throw new ApplicationException("Role creation failed");
                }
            }

            var users = new (string username, string password, string roles)[]
            {
                ("admin@cinesta.ee", "chtulhu", "user,admin"),
                ("user@gmail.com", "qwerty", "user"),
                ("newuser@gmail.com", "123456", "")
            };

            foreach (var userInfo in users)
            {
                var user = userManager.FindByEmailAsync(userInfo.username).Result;
                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = userInfo.username,
                        UserName = userInfo.username,
                        EmailConfirmed = true
                    };
                    var identityResult = userManager.CreateAsync(user, userInfo.password).Result;
                    if (!identityResult.Succeeded) throw new ApplicationException("Cannot create user!");
                }

                if (!string.IsNullOrWhiteSpace(userInfo.roles))
                {
                    var identityResultRole = userManager.AddToRolesAsync(user, userInfo.roles.Split(",")).Result;
                }
            }
        }

        if (config.GetValue<bool>("DataInitialization:SeedData"))
        {
            //TODO
        }
    }
}