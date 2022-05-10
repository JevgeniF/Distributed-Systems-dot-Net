using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class MovieDetails : DomainEntityMetaId
{
    [MaxLength(100)] public string PosterUri { get; set; } = default!;

    [Column(TypeName = "jsonb")] public LangStr Title { get; set; } = new();

    public DateTime Released { get; set; }

    [MaxLength(250)]
    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = new();

    public Guid AgeRatingId { get; set; }
    public AgeRating? AgeRating { get; set; }
    public Guid MovieTypeId { get; set; }
    public MovieType? MovieType { get; set; }
    public ICollection<MovieDbScore>? MovieDbScores { get; set; }
    public ICollection<MovieGenre>? MovieGenres { get; set; }
    public ICollection<Video>? Videos { get; set; }
    public ICollection<UserRating>? UserRatings { get; set; }
    public ICollection<CastInMovie>? CastInMovie { get; set; }
}