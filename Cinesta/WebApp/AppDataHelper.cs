#pragma warning disable CS1591

using System.Security.Claims;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Base.Domain;
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
                ("moderator", "Data moderator"),
                ("newbie", "New service user"),
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
                ("newuser@gmail.com", "Lev", "Tolstoi", "123456", "newbie")
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
            //Age ratings seeding (Estonian system)
            var pere = new AgeRating
            {
                Naming = "PERE",
                AllowedAge = 0
            };
            var ms6 = new AgeRating
            {
                Naming = "MS-6",
                AllowedAge = 6
            };
            var ms12 = new AgeRating
            {
                Naming = "MS-12",
                AllowedAge = 12
            };
            var k12 = new AgeRating
            {
                Naming = "K-12",
                AllowedAge = 12
            };
            var k14 = new AgeRating
            {
                Naming = "K-14",
                AllowedAge = 14
            };
            var k16 = new AgeRating
            {
                Naming = "K-16",
                AllowedAge = 16
            };
            context.AgeRatings.Add(pere);
            context.AgeRatings.Add(ms6);
            context.AgeRatings.Add(ms12);
            context.AgeRatings.Add(k12);
            context.AgeRatings.Add(k14);
            context.AgeRatings.Add(k16);

            var castRoleActor = new CastRole
            {
                Naming = new LangStr("actor", "en")
            };
            var castRoleDirector = new CastRole
            {
                Naming = new LangStr("director", "en")
            };
            context.CastRoles.Add(castRoleActor);
            context.CastRoles.Add(castRoleDirector);
            
            var genreAction = new Genre
            {
                Naming = new LangStr("action", "en")
            };
            var genreHorror = new Genre
            {
                Naming = new LangStr("horror", "en")
            };
            var genreDrama = new Genre
            {
                Naming = new LangStr("drama", "en")
            };
            context.Genres.Add(genreAction);
            context.Genres.Add(genreHorror);
            context.Genres.Add(genreDrama);
            
            var typeMovie = new MovieType
            {
                Naming = new LangStr("movie", "en")
            };
            var typeSeries = new MovieType
            {
                Naming = new LangStr("series", "en")
            };
            context.MovieTypes.Add(typeMovie);
            context.MovieTypes.Add(typeSeries);

            var bruce = new Person
            {
                Name = "Bruce",
                Surname = "Willis"
            };
            var chuck = new Person
            {
                Name = "Chuck",
                Surname = "Norris"
            };
            var steven = new Person
            {
                Name = "Steven",
                Surname = "Spielberg"
            };
            context.Persons.Add(bruce);
            context.Persons.Add(chuck);
            context.Persons.Add(steven);

            var freeSub = new Subscription
            {
                Naming = new LangStr("Free", "en"),
                Price = 0,
                Description = new LangStr("Free sub for empty space", "en"),
                ProfilesCount = 10
            };
            var vipSub = new Subscription
            {
                Naming = new LangStr("VIP", "en"),
                Price = 1000,
                Description = new LangStr("Same as free, but sounds expensive", "en"),
                ProfilesCount = 10
            };
            context.Subscriptions.Add(freeSub);
            context.Subscriptions.Add(vipSub);
            context.SaveChanges();
        }
    }
}