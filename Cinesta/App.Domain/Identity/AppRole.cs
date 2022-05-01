using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppRole : BaseRole
{
    [MinLength(1)] [MaxLength(25)] public string DisplayName { get; set; } = default!;
}