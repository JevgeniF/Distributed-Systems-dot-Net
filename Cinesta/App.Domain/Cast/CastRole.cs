using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain.Cast;

public class CastRole : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Cast.CastRole), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();
}