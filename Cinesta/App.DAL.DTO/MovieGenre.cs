using Base.Domain;

namespace App.DAL.DTO;

public class MovieGenre : DomainEntityId
{
    public Guid? MovieDetailsId { get; set; }

    public MovieDetails? MovieDetails { get; set; }

    public Guid? GenreId { get; set; }

    public Genre? Genre { get; set; }
}