#pragma warning disable CS1591

using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples.CastRoles;

public class PostCastRoleExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "9084e1c6-597a-4f80-80bb-4447696efcb7",
            Naming = new
            {
                en = "director",
                ee = "režisöör",
                ru = "режиссёр"
            }
        };
    }
}