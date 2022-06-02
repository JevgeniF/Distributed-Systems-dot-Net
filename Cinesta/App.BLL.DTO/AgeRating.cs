using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO;

public class AgeRating : DomainEntityId
{
    [MaxLength(20)]
    [Required(ErrorMessageResourceType = typeof(Errors), ErrorMessageResourceName = "fieldIsRequired")]
    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.AgeRating), Name = nameof(Naming))]
    public string Naming { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Errors), ErrorMessageResourceName = "fieldIsRequired")]
    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.AgeRating), Name = nameof(AllowedAge))]
    public int AllowedAge { get; set; }
}