using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class House: DomainEntityId
{
    public string Address { get; set; } = default!;
    public string? Description { get; set; }
    
    [JsonIgnore]
    public ICollection<DAL.DTO.Apartment>? Apartments { get; set; } 
}