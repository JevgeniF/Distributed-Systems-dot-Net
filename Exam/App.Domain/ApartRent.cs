using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class ApartRent: DomainEntityMetaId
{
    public Guid ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
    
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }

    public int? RentMonth { get; set; }
    public int? RentYear { get; set; }
    
    public double Price { get; set; }
    
    public ICollection<RentFixedService>? FixedServices { get; set; }
    public ICollection<RentMonthlyService>? MontlhyServices { get; set; }
}