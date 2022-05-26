using App.Public.DTO.v1;
using Base.Domain;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.Examples;
using Subscription = App.DAL.DTO.Subscription;

namespace WebApp.SwaggerExamples;

public class GetUserSubscriptionExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "c61189fe-45e1-4e60-a581-50f7b01a9782",
            AppUser = new
            {
                AppUserId = "20f2b280-c2ee-446c-a261-f66c4f5cbe40",
                Name = "Johny",
                Surname = "Walker"
            },
            Subscription = new
            {
                SubscriptionId = "85344f7c-4af4-4210-a5ae-f23392530ea0",
                Naming =  new
                {
                    en = "VIP",
                    ee = "VIP",
                    ru = "VIP"
                },
                Description = new
                {
                    en = "Same as free, but sounds expensive",
                    ee = "Sama nagu tasuta, aga maksab palju rohkem",
                    ru = "Такая же как и бесплатная, но звучит богато"
                },
                ProfilesCount = 10,
                Price = 1000
            },
            ExpirationDateTime = "2022-06-26T21:13:05.625514Z"
        };
    }
}