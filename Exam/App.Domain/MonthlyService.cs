using Base.Domain;

namespace App.Domain;

public class MonthlyService: DomainEntityMetaId
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
}