using Base.Domain;

namespace App.Domain;

public class House: DomainEntityMetaId
{
    public string Address { get; set; } = default!;
    public string? Description { get; set; }
    
    public ICollection<Apartment>? Apartments { get; set; } 
}