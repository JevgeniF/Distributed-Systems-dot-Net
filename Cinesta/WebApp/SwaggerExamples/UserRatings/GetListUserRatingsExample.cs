using App.Domain.Identity;
using App.Public.DTO.v1;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.Examples;

namespace WebApp.SwaggerExamples;

public class GetListUserRatingsExample: IExamplesProvider<IEnumerable<object>>
{
    public IEnumerable<object> GetExamples()
    {
        var list = new List<object>
        {
            new
            {
                Id = "c6f24ed8-f592-4f79-8f87-1229e7153b46",
                AppUser = new
                {
                    AppUserId = "20f2b280-c2ee-446c-a261-f66c4f5cbe40",
                    Name = "Johny",
                    Surname = "Walker"
                },
                MovieDetails = new
                {
                    MovieDetailsId = "d35e5c15-296e-4409-a511-628380c2e681",
                    Title = new
                    {
                        en = "Die Hard",
                        ee = "Visa hing",
                        ru = "Крепкий орешек"
                    }
                },
                Rating = 10,
                Comment = "Awesome classic"
            }
        };
        return list;
    }
}