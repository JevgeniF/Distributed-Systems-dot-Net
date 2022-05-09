using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Person : DomainEntityMetaId
{
    [MaxLength(25)]
    public string Name { get; set; } = default!;
    
    [MaxLength(25)]
    public string Surname { get; set; } = default!;
}