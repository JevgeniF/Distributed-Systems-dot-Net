using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Public.DTO.v1;

public class CastRole : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastRole), Name = nameof(Naming))]
    public string Naming { get; set; } = default!;
}