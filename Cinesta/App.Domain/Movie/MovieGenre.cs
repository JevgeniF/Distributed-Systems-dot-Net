using System.ComponentModel.DataAnnotations;
using App.Domain.MovieStandardDetails;
using Base.Domain;

namespace App.Domain.Movie;

public class MovieGenre : DomainEntityMetaId
{
    public Guid? MovieDetailsId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Movie.MovieGenre), Name = nameof(MovieDetails))]

    public MovieDetails? MovieDetails { get; set; }
    
    public Guid? GenreId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Movie.MovieGenre), Name = nameof(Genre))]
    public Genre? Genre { get; set; }
}