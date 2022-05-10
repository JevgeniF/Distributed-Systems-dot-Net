using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class MovieDbScore : DomainEntityId
{
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDbScore), Name = nameof(ImdbId))]
    public string ImdbId { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDbScore), Name = nameof(Score))]
    public double? Score { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDbScore), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}