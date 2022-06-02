#pragma warning disable CS1591

using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples.MovieDetails;

public class GetMovieDetailsExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "d35e5c15-296e-4409-a511-628380c2e681",
            PosterUri = "https://en.wikipedia.org/wiki/Die_Hard#/media/File:Die_Hard_(1988_film)_poster.jpg",
            Title = new
            {
                en = "Die Hard",
                ee = "Visa hing",
                ru = "Крепкий орешек"
            },
            Released = "1988-07-22T00:00:00Z",
            Description = new
            {
                en =
                    "Die Hard follows New York City police detective John McClane (Willis) who is caught up in a terrorist takeover of a Los Angeles skyscraper while visiting his estranged wife.",
                ru =
                    "Крепкий Орешек повествует о детективе полици Нью Йорка - Джоне Макклейне, который попадает в захват террористами небоскрёба в Лос Анджелесе, во время визита к своей жене.",
                ее =
                    "Jõuluõhtul saabub detektiiv John McClane Los Angelesse, et kohtuda oma naisega, kes töötab Nakatomi Plazas, Nakatomi firma pilvelõhkujas. Ettevõtte juhtkond tähistab õnnestunud miljonitehingut. Pidutsemise katkestavad 12 saksa terroristi, kes võtavad kõik hoones viibijad pantvangi."
            },
            AgeRating = new
            {
                Id = "26b2272a-0069-4a64-9c8c-bb02c1a94e91",
                Naming = "K-16",
                AllowedAge = 16
            },
            MovieType = new
            {
                Id = "79a67e38-d715-4d6b-bfdb-ad27c4328cde",
                Naming = new
                {
                    en = "Movie",
                    ee = "Film",
                    ru = "Фильм"
                }
            }
        };
    }
}