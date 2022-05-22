using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class MovieGenre : DomainEntityId
{
    public Guid? MovieDetailsId { get; set; }

    public MovieDetails? MovieDetails { get; set; }

    public Guid? GenreId { get; set; }
    
    public Genre? Genre { get; set; }
}