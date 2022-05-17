using Base.Domain;

namespace App.Public.DTO;

public class Genre : DomainEntityId
{
    public LangStr Naming { get; set; } = default!;
}