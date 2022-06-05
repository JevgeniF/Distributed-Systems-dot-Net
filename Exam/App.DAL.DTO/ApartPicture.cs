
using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class ApartPicture: DomainEntityId
{
    public Guid ApartmentId { get; set; }
    [JsonIgnore]
    public DAL.DTO.Apartment? Apartment { get; set; }
    
    public Guid PictureId { get; set; }
    public Picture? Picture { get; set; }
}