using Base.Domain;

namespace App.Public.DTO;

public class MovieType : DomainEntityId
{
    public string Naming { get; set; } = default!;
}