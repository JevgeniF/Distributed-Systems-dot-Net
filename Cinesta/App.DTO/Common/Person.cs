using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DTO.Common;

public class Person: DomainEntityId<Guid>
{
    [MaxLength(25)]
    [Display(ResourceType = typeof(Resources.App.Domain.Common.Person), Name = nameof(Name))]
    public string Name { get; set; } = default!;

    [MaxLength(25)]
    [Display(ResourceType = typeof(Resources.App.Domain.Common.Person), Name = nameof(Surname))]
    public string Surname { get; set; } = default!;
}