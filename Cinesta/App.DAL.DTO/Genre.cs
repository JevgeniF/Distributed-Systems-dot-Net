using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.DAL.DTO;

public class Genre : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.Genre), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();
}