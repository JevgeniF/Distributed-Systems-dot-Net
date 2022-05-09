using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Video : DomainEntityMetaId
{
    public int? Season { get; set; }

    [Column(TypeName = "jsonb")]
    public LangStr Title { get; set; } = new();

    [MaxLength(100)]
    public string FileUri { get; set; } = default!;
    public TimeOnly Duration { get; set; }

    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = new();
    public Guid? MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}