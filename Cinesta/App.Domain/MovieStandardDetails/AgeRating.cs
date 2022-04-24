using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.MovieStandardDetails;

public class AgeRating : DomainEntityMetaId
{
    [MaxLength(20)]
    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.AgeRating), Name = nameof(Naming))]
    public string Naming { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.AgeRating), Name = nameof(AllowedAge))]
    public int AllowedAge { get; set; }
}