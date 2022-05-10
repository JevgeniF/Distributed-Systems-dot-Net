using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Subscription : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")] public LangStr Naming { get; set; } = new();

    [Column(TypeName = "jsonb")] public LangStr Description { get; set; } = new();

    public int ProfilesCount { get; set; }
    public double Price { get; set; }
}