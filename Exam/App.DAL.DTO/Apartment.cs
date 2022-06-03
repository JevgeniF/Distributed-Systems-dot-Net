
using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class Apartment: DomainEntityId
{
    public int Number { get; set; }
    public int Floor { get; set; }
    public int NumberOfRooms { get; set; }
    public double Size { get; set; }
    public string? Description { get; set; }

    [JsonIgnore]
    public ICollection<Picture>? Pictures { get; set; }
    [JsonIgnore]
    public ICollection<DAL.DTO.Amenity>? Amenities { get; set; }

}