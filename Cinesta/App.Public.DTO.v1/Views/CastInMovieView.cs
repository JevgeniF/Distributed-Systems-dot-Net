using Base.Domain;

namespace App.Public.DTO.v1.Views;

public class CastInMovieView
{
    public CastRole? CastRole;
    public Guid Id;
    public Movie? MovieDetails;
    public Person? Person;
}

public class Movie
{
    public Guid Id;
    public LangStr? Title;
}