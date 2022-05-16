using App.BLL.DTO;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Video : DomainEntityId
{
    public int? Season { get; set; }
    public string Title { get; set; } = default!;
    public string FileUri { get; set; } = default!;
    public TimeOnly Duration { get; set; }
    public string Description { get; set; } = default!;
    public Guid? MovieDetailsId { get; set; }
    public BLL.DTO.MovieDetails? MovieDetails { get; set; }
}