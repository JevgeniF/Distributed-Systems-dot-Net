using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.User;

public class Subscription : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    public LangStr Naming { get; set; } = new();

    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = new();
    public double Price { get; set; }
}