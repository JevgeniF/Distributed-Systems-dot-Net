using Base.Domain;

namespace App.DAL.DTO;

public class FixedService: DomainEntityId
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
}