using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Video : DomainEntityId
{
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Season))]
    public int? Season { get; set; }

    [MaxLength(100)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Title))]
    public LangStr Title { get; set; } = new();

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(FileUri))]
    [MaxLength(150)]
    public string FileUri { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Duration))]
    [DataType(DataType.Time)]
    public DateTime Duration { get; set; }

    [Column(TypeName = "jsonb")]
    [MaxLength(250)]
    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(Description))]
    public LangStr Description { get; set; } = new();

    public Guid? MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Movie.Video), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}