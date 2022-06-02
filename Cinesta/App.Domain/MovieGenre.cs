using Base.Domain;

namespace App.Domain;

public class MovieGenre : DomainEntityMetaId
{
    public Guid? MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
    public Guid? GenreId { get; set; }
    public Genre? Genre { get; set; }
}