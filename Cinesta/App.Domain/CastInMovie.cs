using Base.Domain;

namespace App.Domain;

public class CastInMovie : DomainEntityMetaId
{
    public Guid CastRoleId { get; set; }
    public CastRole? CastRole { get; set; }
    public Guid PersonId { get; set; }
    public Person? Persons { get; set; }
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}