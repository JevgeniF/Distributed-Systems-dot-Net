using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class ProfileMovie : DomainEntityId
{
    public Guid UserProfileId { get; set; }
    
    public UserProfile? UserProfile { get; set; }

    public Guid MovieDetailsId { get; set; }
    
    public MovieDetails? MovieDetails { get; set; }
}