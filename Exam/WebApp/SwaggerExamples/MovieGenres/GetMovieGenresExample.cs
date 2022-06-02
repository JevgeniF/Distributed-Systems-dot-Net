#pragma warning disable CS1591

using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples.MovieGenres;

public class GetMovieGenresExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "9e17b39e-e5f1-44bf-932a-e5d226b0de37",
            MovieDetails = new
            {
                Id = "d35e5c15-296e-4409-a511-628380c2e681",
                Title = new
                {
                    en = "Die Hard",
                    ee = "Visa hing",
                    ru = "Крепкий орешек"
                }
            },
            Genre = new
            {
                Id = "2aabbb3f-0dae-4846-940e-ce9be744f1a9",
                Title = new
                {
                    en = "action",
                    ee = "põnevusfilm",
                    ru = "боевик"
                }
            }
        };
    }
}