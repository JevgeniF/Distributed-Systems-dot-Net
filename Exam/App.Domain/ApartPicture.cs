using Base.Domain;

namespace App.Domain;

public class ApartPicture: DomainEntityMetaId
{
    public Guid ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
    
    public Guid PictureId { get; set; }
    public Picture? Picture { get; set; }
}