using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class UserRating: DomainEntityId
{
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.UserRating), Name = nameof(Rating))]
    public double Rating { get; set; }

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