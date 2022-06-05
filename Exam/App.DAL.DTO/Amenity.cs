using Base.Domain;

namespace App.DAL.DTO;

public class Amenity: DomainEntityId
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}