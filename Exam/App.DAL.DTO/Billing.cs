
using Base.Domain;

namespace App.DAL.DTO;

public class Billing : DomainEntityId
{
    public Guid PersonId { get; set; }
    public Person? Person {get; set;}
    
    public Guid ApartRentId { get; set; }
    public ApartRent? ApartRent { get; set; }

    public int BillingMonth;
    public int BillingYear;
    
    public double TotalSum { get; set; }
}