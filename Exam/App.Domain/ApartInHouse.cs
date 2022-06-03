using Base.Domain;

namespace App.Domain;

public class ApartInHouse: DomainEntityMetaId
{
    public Guid HouseId { get; set; }
    public House? House { get; set; }
    
    public Guid ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
}