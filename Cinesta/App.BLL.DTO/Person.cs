using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class Person : DomainEntityId
{
    [MaxLength(50)] public string Name { get; set; } = default!;

    [MaxLength(50)] public string Surname { get; set; } = default!;
}