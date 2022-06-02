using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.DAL.DTO.Identity;
using Base.Domain;

namespace App.DAL.DTO;

public class Subscription : DomainEntityId
{
    [MaxLength(100)]
    [Column(TypeName = "jsonb")]
    public LangStr Naming { get; set; } = new();

    [MaxLength(250)]
    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = new();

    public int ProfilesCount { get; set; }

    public double Price { get; set; }

    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }
}