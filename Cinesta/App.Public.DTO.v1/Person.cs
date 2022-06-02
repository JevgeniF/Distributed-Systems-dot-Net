using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Person : DomainEntityId
{
    [MaxLength(50)]
    [Display(ResourceType = typeof(Resources.App.Domain.Common.Person), Name = nameof(Name))]
    public string Name { get; set; } = default!;

    [MaxLength(50)]
    [Display(ResourceType = typeof(Resources.App.Domain.Common.Person), Name = nameof(Surname))]
    public string Surname { get; set; } = default!;
}