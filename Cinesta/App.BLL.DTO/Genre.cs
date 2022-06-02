using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.BLL.DTO;

public class Genre : DomainEntityId
{
    [Column(TypeName = "jsonb")] public LangStr Naming { get; set; } = new();
}