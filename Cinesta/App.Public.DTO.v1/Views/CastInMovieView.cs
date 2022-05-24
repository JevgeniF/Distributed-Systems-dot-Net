using Base.Domain;

namespace App.Public.DTO.v1.Views;

public class CastInMovieView
{
    public Guid Id;
    public CastRole? CastRole;
    public Person? Person;
    public Movie? MovieDetails;
}

public class Movie
{
    public Guid Id;
    public LangStr? Title;
}