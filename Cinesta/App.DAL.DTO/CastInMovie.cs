using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class CastInMovie : DomainEntityId
{
    public Guid CastRoleId { get; set; }
    
    public CastRole? CastRole { get; set; }

    public Guid PersonId { get; set; }
    
    public Person? Persons { get; set; }

    public Guid MovieDetailsId { get; set; }
    
    public MovieDetails? MovieDetails { get; set; }
}