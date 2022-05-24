using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples;

public class PostResponseCastInMovieExample: IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "95f44037-90dd-4224-bf80-c65f44a01c9b",
            CastRoleId = "764c9a11-94da-4be8-bc06-d5c55e59e304",
            PersonId = "e3eca745-feb0-4ba7-bd88-389e5ccd2042",
            MovieDetailsId = "d35e5c15-296e-4409-a511-628380c2e681",
        };
    }
}