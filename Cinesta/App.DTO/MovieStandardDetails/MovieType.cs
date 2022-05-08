using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.DTO.MovieStandardDetails;

public class MovieType: DomainEntityId<Guid>
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.MovieType), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();
}