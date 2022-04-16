using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain.MovieStandardDetails;

public class MovieType : DomainEntityMetaId
{
    
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.MovieStandardDetails.MovieType), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();
}