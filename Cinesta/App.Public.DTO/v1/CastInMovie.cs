using System.ComponentModel.DataAnnotations;
using App.BLL.DTO;
using Base.Domain;

namespace App.Public.DTO.v1;

public class CastInMovie : DomainEntityId
{
    public Guid CastRoleId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(CastRole))]
    public App.Public.DTO.v1.CastRole? CastRole { get; set; }

    public Guid PersonId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(Persons))]
    public Person? Persons { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(MovieDetails))]
    public BLL.DTO.MovieDetails? MovieDetails { get; set; }
}