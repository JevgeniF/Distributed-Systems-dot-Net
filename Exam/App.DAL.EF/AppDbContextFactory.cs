using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.DAL.EF;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=cinestadb.postgres.database.azure.com;Port=5432;Database=exam;Username=postgres@cinestadb;Password=DataBase321");

        return new AppDbContext(optionsBuilder.Options);
    }
}