using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.Movie;

public class UserRating : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.UserRating), Name = nameof(Rating))]
    public int Rating { get; set; }

    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.UserRating), Name = nameof(Comment))]
    public LangStr Comment { get; set; } = new();

    public Guid AppUserId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.UserRating), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.UserRating), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}