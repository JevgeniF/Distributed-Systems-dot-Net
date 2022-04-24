using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain.MovieStandardDetails;

public class Genre : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.Genre), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();
}