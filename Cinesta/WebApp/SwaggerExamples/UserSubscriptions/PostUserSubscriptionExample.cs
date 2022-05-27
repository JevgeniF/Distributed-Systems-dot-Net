#pragma warning disable CS1591

using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples.UserSubscriptions;

public class PostUserSubscriptionExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "c61189fe-45e1-4e60-a581-50f7b01a9782", //not required
            AppUserId = "20f2b280-c2ee-446c-a261-f66c4f5cbe40",
            SubscriptionId = "85344f7c-4af4-4210-a5ae-f23392530ea0",
            ExpirationDateTime = "2022-06-26T21:13:05.625514Z" //default 1 month from creation time
        };
    }
}