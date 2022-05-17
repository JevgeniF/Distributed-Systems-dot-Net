using Base.Domain;

namespace App.Public.DTO;

public class CastRole : DomainEntityId
{
    public string Naming { get; set; } = default!;
}