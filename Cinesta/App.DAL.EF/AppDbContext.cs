using App.Domain.Cast;
using App.Domain.Common;
using App.Domain.Identity;
using App.Domain.Movie;
using App.Domain.MovieStandardDetails;
using App.Domain.Profile;
using App.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    //cast
    public DbSet<CastInMovie> CastInMovies { get; set; } = default!;
    public DbSet<CastRole> CastRoles { get; set; } = default!;
    //common
    public DbSet<Person> Persons { get; set; } = default!;
    //movie
    public DbSet<MovieDbScore> MovieDbScores { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<MovieGenre> MovieGenres { get; set; } = default!;
    public DbSet<UserRating> UserRatings { get; set; } = default!;
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
    public DbSet<PaymentDetails> PaymentDetails { get; set; } = default!;
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}