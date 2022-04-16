using Base.Domain;

namespace WebApp.DTO;

public class MovieTypeDto : DomainEntityId
{
    public string Naming { get; set; } = default!;
}