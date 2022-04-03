using Base.Domain;

namespace App.Domain.Movie;

public class MovieDbScore : DomainEntityMetaId
{
    public double Score { get; set; }
}