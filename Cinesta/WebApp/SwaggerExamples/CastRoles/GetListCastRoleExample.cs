using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.Examples;

namespace WebApp.SwaggerExamples;

public class GetListCastRoleExample: IExamplesProvider<IEnumerable<object>>
{
    public IEnumerable<object> GetExamples()
    {
        var list = new List<object>
        {
            new
            {
                Id = "764c9a11-94da-4be8-bc06-d5c55e59e304",
                Naming = new
                {
                    en = "actor",
                    ee = "näitleja",
                    ru = "актёр"
                }
            },
            new
            {
                Id = "9084e1c6-597a-4f80-80bb-4447696efcb7",
                Naming = new
                {
                    en = "director",
                    ee = "režisöör",
                    ru = "режиссёр"
                }
            }
        };
        return list;
    }
}