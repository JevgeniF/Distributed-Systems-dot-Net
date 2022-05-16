using Base.Domain;

namespace App.Public.DTO.v1;

public class Genre : DomainEntityId
{
    public LangStr Naming { get; set; } = default!;
}