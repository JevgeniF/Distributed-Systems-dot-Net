using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Movie;

public class Video : DomainEntityMetaId
{
    public int? Season { get; set; }
    [MaxLength(100)] public string Title { get; set; } = default!;
    [MaxLength(100)] public string FileUri { get; set; } = default!;
    public TimeOnly Duration { get; set; }
    [MaxLength(250)] public string Description { get; set; } = default!;
}