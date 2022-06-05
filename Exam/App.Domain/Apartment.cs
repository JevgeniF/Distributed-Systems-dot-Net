using Base.Domain;

namespace App.Domain;

public class Apartment: DomainEntityMetaId
{
    public int Number { get; set; }
    public int Floor { get; set; }
    public int NumberOfRooms { get; set; }
    public double Size { get; set; }
    public string? Description { get; set; }

    public ICollection<Picture>? Pictures { get; set; }
    public ICollection<Amenity>? Amenities { get; set; }

}