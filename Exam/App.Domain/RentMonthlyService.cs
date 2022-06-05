using Base.Domain;

namespace App.Domain;

public class RentMonthlyService: DomainEntityMetaId
{
    public Guid ApartRentId { get; set; }
    public ApartRent? ApartRent { get; set; }
    
    public Guid MonthlyServiceId { get; set; }
    public MonthlyService? MonthlyService { get; set; }
    
    public double Consumption { get; set; }
}