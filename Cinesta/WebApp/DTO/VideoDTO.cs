using App.Domain.Movie;
using Base.Domain;

namespace WebApp.DTO;

public class VideoDto : DomainEntityId
{
    public int? Season { get; set; }
    public string Title { get; set; } = default!;
    public string FileUri { get; set; } = default!;
    public TimeOnly Duration { get; set; }
    public string Description { get; set; } = default!;
    public Guid? MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}