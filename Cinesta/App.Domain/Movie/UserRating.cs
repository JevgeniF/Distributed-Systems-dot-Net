using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.Movie;

public class UserRating : DomainEntityMetaId
{
    public int Rating { get; set; }
    [MaxLength(250)] public string Comment { get; set; } = default!;
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}