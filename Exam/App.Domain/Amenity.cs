using Base.Domain;

namespace App.Domain;

public class Amenity: DomainEntityMetaId
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}