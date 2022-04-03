using App.Domain.Common;
using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Cast;

public class CastInMovie : DomainEntityMetaId
{
    public Guid CastRoleId { get; set; }
    public CastRole? CastRole { get; set; }
    
    public ICollection<Person>? PersonsCollection { get; set; }
    
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}