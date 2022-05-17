using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.Public.DTO;

public class UserRating : DomainEntityId
{
    public double Rating { get; set; }
    public string Comment { get; set; } = default!;
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid MovieDetailsId { get; set; }
    public BLL.DTO.MovieDetails? MovieDetails { get; set; }
}