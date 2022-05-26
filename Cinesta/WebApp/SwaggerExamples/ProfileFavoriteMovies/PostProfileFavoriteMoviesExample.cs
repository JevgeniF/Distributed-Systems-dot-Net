using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples;

public class PostProfileFavoriteMoviesExample: IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "a76521a6-7ee3-4584-ab50-669477e569d5",
            UserProfileId = "c6f24ed8-f592-4f79-8f87-1229e7153b46",
            MovieDetailsId = "d35e5c15-296e-4409-a511-628380c2e681"
        };
    }
}