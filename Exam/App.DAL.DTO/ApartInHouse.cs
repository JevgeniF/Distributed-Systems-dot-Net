using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class ApartInHouse: DomainEntityId
{
    public Guid HouseId { get; set; }
    [JsonIgnore]
    public House? House { get; set; }
    
    public Guid ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
}