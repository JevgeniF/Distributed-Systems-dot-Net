using Base.Domain;

namespace App.DAL.DTO;

public class MonthlyService: DomainEntityMetaId
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
}