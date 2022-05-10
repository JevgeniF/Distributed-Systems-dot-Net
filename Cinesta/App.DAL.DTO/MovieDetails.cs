using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.DAL.DTO;

public class MovieDetails : DomainEntityId
{
    [MaxLength(100)]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDetails), Name = nameof(PosterUri))]
    public string PosterUri { get; set; } = default!;

    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDetails), Name = nameof(Title))]
    public LangStr Title { get; set; } = new();

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDetails), Name = nameof(Released))]
    public DateTime Released { get; set; }

    [MaxLength(250)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDetails), Name = nameof(Description))]
    public LangStr Description { get; set; } = new();

    public Guid AgeRatingId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDetails), Name = nameof(AgeRating))]
    public AgeRating? AgeRating { get; set; }

    public Guid MovieTypeId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.MovieDetails), Name = nameof(MovieType))]
    public MovieType? MovieType { get; set; }

    public ICollection<MovieDbScore>? MovieDbScores { get; set; }

    public ICollection<MovieGenre>? MovieGenres { get; set; }

    public ICollection<Video>? Videos { get; set; }

    public ICollection<UserRating>? UserRatings { get; set; }

    public ICollection<CastInMovie>? CastInMovie { get; set; }
}