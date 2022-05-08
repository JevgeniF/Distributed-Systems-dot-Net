using System.ComponentModel.DataAnnotations;
using App.Domain.MovieStandardDetails;
using Base.Domain;

namespace App.Domain.Movie;

public class MovieGenre : DomainEntityMetaId
{
    public Guid? MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
    public Guid? GenreId { get; set; }
    public Genre? Genre { get; set; }
}