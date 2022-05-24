using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

    public class AgeRating : DomainEntityId
    {
        [MaxLength(20)]
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Errors), ErrorMessageResourceName = "fieldIsRequired")]
        [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.AgeRating), Name = nameof(Naming))]
        public string Naming { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Base.Resources.Errors), ErrorMessageResourceName = "fieldIsRequired")]
        [Display(ResourceType = typeof(Resources.App.Domain.MovieStandardDetails.AgeRating), Name = nameof(AllowedAge))]
        public int AllowedAge { get; set; }
    }
