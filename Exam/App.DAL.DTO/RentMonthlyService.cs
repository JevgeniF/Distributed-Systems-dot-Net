using System.Text.Json.Serialization;
using Base.Domain;

namespace App.DAL.DTO;

public class RentMonthlyService: DomainEntityMetaId
{
    public Guid ApartRentId { get; set; }
    [JsonIgnore]
    public ApartRent? ApartRent { get; set; }
    
    public Guid MonthlyServiceId { get; set; }
    public DAL.DTO.MonthlyService? MonthlyService { get; set; }
    
    public double Consumption { get; set; }
}