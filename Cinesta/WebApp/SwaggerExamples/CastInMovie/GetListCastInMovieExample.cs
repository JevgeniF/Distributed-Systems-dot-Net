using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.Examples;

namespace WebApp.SwaggerExamples;

public class GetListCastInMovieExample: IExamplesProvider<IEnumerable<object>>
{
    public IEnumerable<object> GetExamples()
    {
        List<object> list = new List<object>();
        list.Add(new {
            Id = "95f44037-90dd-4224-bf80-c65f44a01c9b",
            CastRole = new
            {
                Id = "764c9a11-94da-4be8-bc06-d5c55e59e304",
                Naming = new
                {
                    en = "actor",
                    ee = "näitleja",
                    ru = "актёр"
                }
            },
            Person = new
            {
                Id = "e3eca745-feb0-4ba7-bd88-389e5ccd2042",
                Name = "Bruce",
                Surname = "Willis"
            },
            MovieDetails = new
            {
                Id = "d35e5c15-296e-4409-a511-628380c2e681",
                Title = new
                {
                    en = "Die Hard",
                    ee = "Visa hing",
                    ru = "Крепкий орешек"
                }
            }
        });
        return list;
    }
}