using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class MovieGenre: DomainEntityId
{
    public Guid? MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieGenre), Name = nameof(MovieDetails))]

    public MovieDetails? MovieDetails { get; set; }

    public Guid? GenreId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieGenre), Name = nameof(Genre))]
    public Genre? Genre { get; set; }
}