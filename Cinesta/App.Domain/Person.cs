using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Person : DomainEntityMetaId
{
    [MaxLength(50)]
    public string Name { get; set; } = default!;

    [MaxLength(50)]
    public string Surname { get; set; } = default!;
}