using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Base.Domain;

namespace App.Public.DTO.v1;

[DataContract]
public class CastInMovie : DomainEntityId
{
    [DataMember]
    public Guid CastRoleId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(CastRole))]
    public CastRole? CastRole { get; set; }

    [DataMember]
    public Guid PersonId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(Persons))]
    public Person? Persons { get; set; }

    [DataMember]
    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Cast.CastInMovie), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}