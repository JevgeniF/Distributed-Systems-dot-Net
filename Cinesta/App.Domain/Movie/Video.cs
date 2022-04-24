using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain.Movie;

public class Video : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Season))]
    public int? Season { get; set; }

    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Title))]
    public LangStr Title { get; set; } = new();

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(FileUri))]
    [MaxLength(100)]
    public string FileUri { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Duration))]
    public TimeOnly Duration { get; set; }

    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Description))]
    public LangStr Description { get; set; } = new();

    public Guid? MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}