using App.Public.DTO.v1;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.Examples;

namespace WebApp.SwaggerExamples;

public class GetProfileMoviesExample: IExamplesProvider<IEnumerable<object>>
{
    public IEnumerable<object> GetExamples()
    {
        var list = new List<object>
        {
            new
            {
                Id = "a76521a6-7ee3-4584-ab50-669477e569d5",
                UserProfileId = "c6f24ed8-f592-4f79-8f87-1229e7153b46",
                MovieDetails = new
                {
                    MovieDetailsId = "d35e5c15-296e-4409-a511-628380c2e681",
                    PosterUri = "https://en.wikipedia.org/wiki/Die_Hard#/media/File:Die_Hard_(1988_film)_poster.jpg",
                    Title = new
                    {
                        en = "Die Hard",
                        ee = "Visa hing",
                        ru = "Крепкий орешек"
                    }
                }
            }
        };
        return list;
    }
}