using System.Dynamic;
using Base.Domain;

namespace App.Domain;

public class RentFixedService: DomainEntityMetaId
{
    public Guid ApartRentId { get; set; }
    public ApartRent? ApartRent { get; set; }
    
    public Guid FixedServiceId { get; set; }
    public FixedService? FixedService { get; set; }
}