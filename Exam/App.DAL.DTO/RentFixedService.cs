using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class RentFixedService: DomainEntityId
{
    public Guid ApartRentId { get; set; }
    [JsonIgnore]
    public DAL.DTO.ApartRent? ApartRent { get; set; }
    
    public Guid FixedServiceId { get; set; }
    public DAL.DTO.FixedService? FixedService { get; set; }
}