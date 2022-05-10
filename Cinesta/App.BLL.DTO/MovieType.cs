using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.BLL.DTO;

public class MovieType : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.MovieType), Name = nameof(Naming))]
    public LangStr Naming { get; set; } = new();
}