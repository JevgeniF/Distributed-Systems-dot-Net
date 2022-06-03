using System.Text.Json;
using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    //identity
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;

    //other
    public DbSet<Amenity> Amenities { get; set; } = default!;
    public DbSet<ApartAmenity> ApartAmenities { get; set; } = default!;
    public DbSet<Apartment> Apartments { get; set; } = default!;
    public DbSet<ApartPicture> ApartPictures { get; set; } = default!;
    public DbSet<ApartRent> ApartRents { get; set; } = default!;
    public DbSet<Billing> Billings { get; set; } = default!;
    public DbSet<FixedService> FixedServices { get; set; } = default!;
    public DbSet<House> Houses { get; set; } = default!;
    public DbSet<Person> Persons { get; set; } = default!;
    public DbSet<Picture> Pictures { get; set; } = default!;
    public DbSet<RentFixedService> RentFixedServices { get; set; } = default!;
    public DbSet<MonthlyService> MonthlyServices {get; set; } = default!;
    public DbSet<RentMonthlyService> RentMonthlyServices { get; set; } = default!;
    public DbSet<ApartInHouse> ApartInHouse { get; set; } = default!;


    private static string SerializeLangStr(LangStr langStr)
    {
        return JsonSerializer.Serialize(langStr);
    }

    private static LangStr DeserializeLangStr(string jsonStr)
    {
        return JsonSerializer.Deserialize<LangStr>(jsonStr) ?? new LangStr();
    }

    public override int SaveChanges()
    {
        FixEntities(this);

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        FixEntities(this);

        return base.SaveChangesAsync(cancellationToken);
    }


    private void FixEntities(AppDbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);


        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        }
    }
}