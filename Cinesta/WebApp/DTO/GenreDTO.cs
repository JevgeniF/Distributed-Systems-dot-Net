using Base.Domain;

namespace WebApp.DTO;

public class GenreDto : DomainEntityId
{
    public LangStr Naming { get; set; } = default!;
}