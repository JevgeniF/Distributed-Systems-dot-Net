
using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class ApartRent: DomainEntityId
{
    public Guid ApartmentId { get; set; }
    [JsonIgnore]
    public DAL.DTO.Apartment? Apartment { get; set; }
    
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }

    public int? RentMonth { get; set; }
    public int? RentYear { get; set; }
    
    public double Price { get; set; }
    
    [JsonIgnore]
    public ICollection<RentFixedService>? FixedServices { get; set; }
    [JsonIgnore]
    public ICollection<RentMonthlyService>? MontlhyServices { get; set; }

}