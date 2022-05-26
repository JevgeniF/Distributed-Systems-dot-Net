using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples;

public class PostMovieDBScoreExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "168662c2-8786-4c39-9ac3-abeccf98597d",
            ImdbId = "tt0095016",
            Score = 9,
            MovieDetailsId = "d35e5c15-296e-4409-a511-628380c2e681"
        };
    }
}