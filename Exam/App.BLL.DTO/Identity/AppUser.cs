using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO.Identity;

public class AppUser : DomainEntityId
{
    [MinLength(1)] [MaxLength(50)] public string Name { get; set; } = default!;
    [MinLength(1)] [MaxLength(50)] public string Surname { get; set; } = default!;
}