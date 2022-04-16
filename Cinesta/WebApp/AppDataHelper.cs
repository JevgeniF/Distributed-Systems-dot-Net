using App.DAL.EF;
using App.Domain.Cast;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public static class AppDataHelper
{
    public static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Services error. No DB context");
        }

        //TODO - check  database state
        //can't connect - wrong address
        //can't connect - wrong user/pass
        //can connect - no database
        //can connect - is database

        if (config.GetValue<bool>("DataInitialization:DropDatabase"))
        {
            context.Database.EnsureDeleted();
        }

        if (config.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            context.Database.Migrate();
        }

        if (config.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            //TODO
        }

        if (config.GetValue<bool>("DataInitialization:SeedData"))
        {
           //TODO
        }
    }

}