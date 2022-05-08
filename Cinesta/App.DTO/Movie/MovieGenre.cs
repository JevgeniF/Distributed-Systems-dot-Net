using System.ComponentModel.DataAnnotations;
using App.DTO.MovieStandardDetails;
using Base.Domain;

namespace App.DTO.Movie;

public class MovieGenre: DomainEntityId<Guid>
{
    public Guid? MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieGenre), Name = nameof(MovieDetails))]

    public MovieDetails? MovieDetails { get; set; }

    public Guid? GenreId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieGenre), Name = nameof(Genre))]
    public Genre? Genre { get; set; }
}