using System.ComponentModel.DataAnnotations;
using App.Domain.Cast;
using Base.Domain;

namespace App.Domain.Common;

public class Person : DomainEntityMetaId
{
    [MaxLength(25)] public string Name { get; set; } = default!;
    [MaxLength(25)] public string Surname { get; set; } = default!;
}