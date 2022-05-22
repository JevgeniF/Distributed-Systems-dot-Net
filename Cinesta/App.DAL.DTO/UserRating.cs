using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.DAL.DTO.Identity;
using Base.Domain;

namespace App.DAL.DTO;

public class UserRating : DomainEntityId
{
    public double Rating { get; set; }

    [MaxLength(500)]
    [Column(TypeName = "jsonb")]
    public LangStr Comment { get; set; } = new();

    public Guid AppUserId { get; set; }
    
    public AppUser? AppUser { get; set; }

    public Guid MovieDetailsId { get; set; }
    
    public MovieDetails? MovieDetails { get; set; }
}