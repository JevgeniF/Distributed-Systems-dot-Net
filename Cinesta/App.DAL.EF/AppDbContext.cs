using System.Text.Json;
using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    //identity
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;

    //cast
    public DbSet<CastInMovie> CastInMovies { get; set; } = default!;

    public DbSet<CastRole> CastRoles { get; set; } = default!;

    //common
    public DbSet<Person> Persons { get; set; } = default!;

    //movie
    public DbSet<MovieDbScore> MovieDbScores { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<MovieGenre> MovieGenres { get; set; } = default!;

    public DbSet<Video> Videos { get; set; } = default!;

    //movie standardized details
    public DbSet<AgeRating> AgeRatings { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;

    public DbSet<MovieType> MovieTypes { get; set; } = default!;

    //profile
    public DbSet<ProfileMovie> ProfileMovies { get; set; } = default!;
    public DbSet<ProfileFavoriteMovie> ProfileFavoriteMovies { get; set; } = default!;

    public DbSet<UserProfile> UserProfiles { get; set; } = default!;

    //user
    public DbSet<Subscription> Subscriptions { get; set; } = default!;
    public DbSet<UserSubscription> UserSubscriptions { get; set; } = default!;
    public DbSet<PaymentDetails> PaymentDetails { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            builder.Entity<CastRole>().Property(a => a.Naming)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<Genre>().Property(a => a.Naming)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<MovieDetails>().Property(a => a.Title)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<MovieDetails>().Property(a => a.Description)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<MovieType>().Property(a => a.Naming)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<Subscription>().Property(a => a.Naming)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<Subscription>().Property(a => a.Description)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<Video>().Property(a => a.Title)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
            builder.Entity<Video>().Property(a => a.Description)
                .HasConversion(n => SerializeLangStr(n),
                    n => DeserializeLangStr(n));
        }
    }


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