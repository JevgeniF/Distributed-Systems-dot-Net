﻿#pragma warning disable CS1591

using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples.PaymentDetails;

public class GetPaymentDetailsExample : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "bac4509c-82b5-4839-9325-f23638265395",
            CardType = "MasterCard",
            CardNumber = "5123000000000000",
            ValidDate = "2022-10-01T00:00:00Z",
            SecurityCode = "000",
            AppUser = new
            {
                AppUserId = "20f2b280-c2ee-446c-a261-f66c4f5cbe40",
                Name = "Johny",
                Surname = "Walker"
            }
        };
    }
}