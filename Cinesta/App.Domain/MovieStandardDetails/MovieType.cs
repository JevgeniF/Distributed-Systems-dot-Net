using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain.MovieStandardDetails;

public class MovieType : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    public LangStr Naming { get; set; } = new();
}