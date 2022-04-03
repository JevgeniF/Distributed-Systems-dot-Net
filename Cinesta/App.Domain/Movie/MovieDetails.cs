using System.ComponentModel.DataAnnotations;
using App.Domain.Cast;
using App.Domain.MovieStandardDetails;
using Base.Domain;

namespace App.Domain.Movie;

public class MovieDetails : DomainEntityMetaId
{
    [MaxLength (100)]
    public string PosterUri { get; set; } = default!;
    [MaxLength (100)]
    public string Title { get; set; } = default!;
    public DateOnly Released { get; set; }
    [MaxLength (250)]
    public string Description { get; set; } = default!;
    
    public Guid AgeRatingId { get; set; }
    public AgeRating? AgeRating { get; set; }
    
    public Guid MovieTypeId { get; set; }
    public MovieType? MovieType { get; set; }
    
    public Guid MovieDbScoreId { get; set; }
    public MovieDbScore? MovieMovieDbScore { get; set; }

    public ICollection<Genre>? Genres { get; set; }

    public ICollection<Video>? Videos { get; set; }
    
    public ICollection<UserRating>? UserRatings { get; set; }
    
    public ICollection<CastInMovie>? CastInMovie { get; set; }
}