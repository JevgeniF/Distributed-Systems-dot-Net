using System.ComponentModel.DataAnnotations;
using App.DTO.Common;
using App.DTO.Movie;
using Base.Domain;

namespace App.DTO.Cast;

public class CastInMovie: DomainEntityId<Guid>
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