using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Base.Domain;

namespace App.Public.DTO.v1;

public class CastInMovie : DomainEntityId
{
   [Required]
    public Guid CastRoleId { get; set; }
    public CastRole? CastRole { get; set; }
    [Required]
    public Guid PersonId { get; set; }
    public Person? Persons { get; set; }
    [Required]
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}