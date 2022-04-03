using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Cast;

public class CastRole : DomainEntityMetaId
{
    [MaxLength (35)]
    public string Naming { get; set; } = default!;
}