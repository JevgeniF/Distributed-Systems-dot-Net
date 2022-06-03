using Base.Domain;

namespace App.Domain;

public class ApartAmenity: DomainEntityMetaId
{
    public Guid ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
    
    public Guid AmenityId{ get; set; }
    public Amenity? Amenity { get; set; }
}