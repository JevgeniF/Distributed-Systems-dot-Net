using System.ComponentModel.DataAnnotations;
using App.Domain.Common;
using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Cast;

public class CastInMovie : DomainEntityMetaId
{
    public Guid CastRoleId { get; set; }
    public CastRole? CastRole { get; set; }
    public Guid PersonId { get; set; }
    public Person? Persons { get; set; }
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}