using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Movie;

public class MovieDbScore : DomainEntityMetaId
{
    public string ImdbId { get; set; } = default!; 
    public double? Score { get; set; }
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}