using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.DAL.DTO;

public class CastRole : DomainEntityId
{
    [Column(TypeName = "jsonb")] public LangStr Naming { get; set; } = new();
}