using System.Security.Claims;
using App.DAL.EF;
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

            var roles = new (string name, string displayName)[]
            {
                ("admin", "System administrator"),
                ("user", "Service user")
            };

            foreach (var roleInfo in roles)
            {
                var role = roleManager.FindByNameAsync(roleInfo.name).Result;
                if (role == null)
                {
                    var identityResult = roleManager.CreateAsync(new AppRole
                    {
                        Name = roleInfo.name,
                        DisplayName = roleInfo.displayName
                    }).Result;
                    if (!identityResult.Succeeded) throw new ApplicationException("Role creation failed");
                }
            }

            var users = new (string username, string name, string surname, string password, string roles)[]
            {
                ("admin@cinesta.ee", "Jevgeni", "Fenko", "chtulhu", "user,admin"),
                ("user@gmail.com", "Oskar", "Luts", "qwerty", "user"),
                ("newuser@gmail.com", "Lev", "Tolstoi", "123456", "")
            };

            foreach (var userInfo in users)
            {
                var user = userManager.FindByEmailAsync(userInfo.username).Result;
                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = userInfo.username,
                        Name = userInfo.name,
                        Surname = userInfo.surname,
                        UserName = userInfo.username,
                        EmailConfirmed = true
                    };
                    var identityResult = userManager.CreateAsync(user, userInfo.password).Result;
                    identityResult = userManager.AddClaimAsync(user, new Claim("aspnet.name", user.Name)).Result;
                    identityResult = userManager.AddClaimAsync(user, new Claim("aspnet.surname", user.Surname)).Result;
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