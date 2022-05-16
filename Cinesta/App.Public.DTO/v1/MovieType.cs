using Base.Domain;

namespace App.Public.DTO.v1;

public class MovieType : DomainEntityId
{
    public string Naming { get; set; } = default!;
}