using System.ComponentModel.DataAnnotations;
using App.Domain.Common;
using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Cast
{
    public class CastInMovie : DomainEntityMetaId
    {
        public Guid CastRoleId { get; set; }
        
        [Display(ResourceType = typeof(App.Resources.App.Domain.Cast.CastInMovie), Name = nameof(CastRole))]
        public CastRole? CastRole { get; set; }
    
        public Guid PersonId { get; set; }
        
        [Display(ResourceType = typeof(App.Resources.App.Domain.Cast.CastInMovie), Name = nameof(Persons))]
        public Person? Persons { get; set; }
    
        public Guid MovieDetailsId { get; set; }
        
        [Display(ResourceType = typeof(App.Resources.App.Domain.Cast.CastInMovie), Name = nameof(MovieDetails))]
        public MovieDetails? MovieDetails { get; set; }
    }
}