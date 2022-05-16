using App.BLL.DTO;
using Base.Domain;

namespace App.Public.DTO.v1;

public class MovieDetails : DomainEntityId
{
    public string PosterUri { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime Released { get; set; }
    public string Description { get; set; } = default!;
    public Guid AgeRatingId { get; set; }
    public AgeRating? AgeRating { get; set; }
    public Guid MovieTypeId { get; set; }
    public BLL.DTO.MovieType? MovieType { get; set; }
    public ICollection<MovieDbScore>? MovieDbScores { get; set; }
    public ICollection<MovieGenre>? Genres { get; set; }
    public ICollection<BLL.DTO.Video>? Videos { get; set; }
    public ICollection<BLL.DTO.UserRating>? UserRatings { get; set; }
    public ICollection<BLL.DTO.CastInMovie>? CastInMovie { get; set; }
}