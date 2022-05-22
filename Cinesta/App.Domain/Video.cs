using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Video : DomainEntityMetaId
{
    public int? Season { get; set; }

    [MaxLength(100)]
    [Column(TypeName = "jsonb")]
    public LangStr Title { get; set; } = new();
    
    [MaxLength(150)]
    public string FileUri { get; set; } = default!;
    
    [DataType(DataType.Time)] public DateTime Duration { get; set; }

    [Column(TypeName = "jsonb")]
    [MaxLength(250)]
    public LangStr Description { get; set; } = new();

    public Guid? MovieDetailsId { get; set; }
    
    public MovieDetails? MovieDetails { get; set; }
}