using App.BLL.DTO;
using Base.Domain;

namespace WebApp.DTO;

public class MovieDetailsDto : DomainEntityId
{
    public string PosterUri { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime Released { get; set; }
    public string Description { get; set; } = default!;
    public Guid AgeRatingId { get; set; }
    public AgeRating? AgeRating { get; set; }
    public Guid MovieTypeId { get; set; }
    public MovieType? MovieType { get; set; }
    public ICollection<MovieDbScore>? MovieDbScores { get; set; }
    public ICollection<MovieGenre>? Genres { get; set; }
    public ICollection<Video>? Videos { get; set; }
    public ICollection<UserRating>? UserRatings { get; set; }
    public ICollection<CastInMovie>? CastInMovie { get; set; }
}