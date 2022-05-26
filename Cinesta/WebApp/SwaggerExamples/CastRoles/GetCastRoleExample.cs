using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples;

public class GetCastRoleExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "764c9a11-94da-4be8-bc06-d5c55e59e304",
            Naming = new
            {
                en = "actor",
                ee = "näitleja",
                ru = "актёр"
            }
        };
    }
}