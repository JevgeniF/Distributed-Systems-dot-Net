using Base.Domain;

namespace App.Domain;

public class FixedService: DomainEntityMetaId
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
}