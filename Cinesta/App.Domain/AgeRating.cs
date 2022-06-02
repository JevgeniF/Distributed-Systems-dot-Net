using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class AgeRating : DomainEntityMetaId
{
    [MaxLength(20)] public string Naming { get; set; } = default!;
    public int AllowedAge { get; set; }
}