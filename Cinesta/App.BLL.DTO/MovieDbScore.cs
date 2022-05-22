using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class MovieDbScore : DomainEntityId
{
    public string ImdbId { get; set; } = default!;
    
    public double? Score { get; set; }

    public Guid MovieDetailsId { get; set; }
    
    public MovieDetails? MovieDetails { get; set; }
}