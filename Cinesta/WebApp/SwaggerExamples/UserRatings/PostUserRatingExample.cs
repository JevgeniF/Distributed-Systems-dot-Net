using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples;

public class PostUserRatingExample: IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "c6f24ed8-f592-4f79-8f87-1229e7153b46",
            MovieDetailsId = "d35e5c15-296e-4409-a511-628380c2e681",
            Rating = 10,
            Comment = "Awesome classic",
            AppUserId = "20f2b280-c2ee-446c-a261-f66c4f5cbe40"
        };
    }
}