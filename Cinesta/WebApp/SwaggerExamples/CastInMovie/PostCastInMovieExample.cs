using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples;

public class PostCastInMovieExample: IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "2023829e-177a-4cac-9bf0-7f43275aff48",
            CastRoleId = "764c9a11-94da-4be8-bc06-d5c55e59e304",
            PersonId = "e3eca745-feb0-4ba7-bd88-389e5ccd2042",
            MovieDetailsId = "d35e5c15-296e-4409-a511-628380c2e681"
        };
    }
}