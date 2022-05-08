using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.Movie;

public class UserRating : DomainEntityMetaId
{
    public double Rating { get; set; }

    [Column(TypeName = "jsonb")]
    public LangStr Comment { get; set; } = new();
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}