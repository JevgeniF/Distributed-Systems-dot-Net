using Base.Domain;

namespace App.Domain;

public class Billing: DomainEntityMetaId
{
    public Guid PersonId { get; set; }
    public Person? Person {get; set;}
    public Guid ApartRentId { get; set; }
    public ApartRent? ApartRent { get; set; }

    public int BillingMonth;
    public int BillingYear;
    
    public double TotalSum { get; set; }
}