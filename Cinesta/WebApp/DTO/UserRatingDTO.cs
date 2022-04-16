using App.Domain.Identity;
using App.Domain.Movie;
using Base.Domain;

namespace WebApp.DTO;

public class UserRatingDto : DomainEntityId
{
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}