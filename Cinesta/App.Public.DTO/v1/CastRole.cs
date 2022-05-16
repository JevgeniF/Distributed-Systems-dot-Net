using Base.Domain;

namespace App.Public.DTO.v1;

public class CastRole : DomainEntityId
{
    public string Naming { get; set; } = default!;
}