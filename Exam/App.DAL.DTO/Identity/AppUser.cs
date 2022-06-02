using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO.Identity;

public class AppUser : DomainEntityId
{
    public Guid? PersonId { get; set; } = default!;

    [MinLength(1)] [MaxLength(50)] public string Name { get; set; } = default!;
    [MinLength(1)] [MaxLength(50)] public string Surname { get; set; } = default!;
}