using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Genre : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    public LangStr Naming { get; set; } = new();
}