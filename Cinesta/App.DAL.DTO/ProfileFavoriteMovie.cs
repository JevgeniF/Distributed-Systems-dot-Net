using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class ProfileFavoriteMovie : DomainEntityId
{
    public Guid UserProfileId { get; set; }
    
    public UserProfile? UserProfile { get; set; }

    public Guid MovieDetailsId { get; set; }
    
    public MovieDetails? MovieDetails { get; set; }
}