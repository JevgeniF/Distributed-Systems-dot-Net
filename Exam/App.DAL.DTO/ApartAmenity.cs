using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class ApartAmenity: DomainEntityId
{
    public Guid ApartmentId { get; set; }
    [JsonIgnore]
    public Apartment? Apartment { get; set; }
    
    public Guid AmenityId{ get; set; }
    public DAL.DTO.Amenity? Amenity { get; set; }
}