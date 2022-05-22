using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class AgeRating : DomainEntityId
{
    [MaxLength(20)] public string Naming { get; set; } = default!;

    public int AllowedAge { get; set; }
}