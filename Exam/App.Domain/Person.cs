using Base.Domain;

namespace App.Domain;

public class Person: DomainEntityMetaId
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}