using Base.Domain;

namespace WebApp.DTO;

public class CastRoleDto : DomainEntityId
{
    public string Naming { get; set; } = default!;
}