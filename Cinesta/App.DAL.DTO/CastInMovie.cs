using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class CastInMovie : DomainEntityId
{
    public Guid CastRoleId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(CastRole))]
    public CastRole? CastRole { get; set; }

    public Guid PersonId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(Persons))]
    public Person? Persons { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}