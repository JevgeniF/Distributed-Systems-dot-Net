using App.BLL.DTO;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class UserRating : DomainEntityId
{
    public double Rating { get; set; }
    public string Comment { get; set; } = default!;
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid MovieDetailsId { get; set; }
    public BLL.DTO.MovieDetails? MovieDetails { get; set; }
}