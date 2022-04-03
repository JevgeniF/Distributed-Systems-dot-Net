using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.MovieStandardDetails;

public class MovieGenre : DomainEntityMetaId
{
    [MaxLength(20)] public string Naming { get; set; } = default!;
}