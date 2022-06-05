using Base.Domain;

namespace App.DAL.DTO;

public class Person: DomainEntityId
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}