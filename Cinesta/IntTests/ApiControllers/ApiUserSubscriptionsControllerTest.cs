using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiUserSubscriptionsControllerTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private const string Culture = "?culture=en-GB";

    public ApiUserSubscriptionsControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }

    //SCENARIO
    [Fact]
    public async Task ApiUserSubscriptionsControllerIntTestingScenario()
    {
        await Test01_PostUserSubscription_Returns_Status_OK_And_UserSubscriptionId();
        await Test02_GetUserSubscriptions_Returns_Status_OK_UserSubscription_For_Current_User();
        await Test03_DeleteUserSubscription_Returns_Status_OK_In_Case_Of_Success();
    }
    
    //POST METHOD
    [Fact]
    public async Task Test01_PostUserSubscription_Returns_Status_OK_And_UserSubscriptionId()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        var apiSubscrRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiSubscrRequest.RequestUri = new Uri(ApiUrl + "Subscriptions/" + Culture);

        var apiSubscrResponse = await _client.SendAsync(apiSubscrRequest);

        var apiSubscrContent = await apiSubscrResponse.Content.ReadAsStringAsync();
        var resultSubscrData = JsonSerializer.Deserialize<List<Subscription>>(apiSubscrContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var subscriptionId = resultSubscrData![0].Id;

        var data = IntTestsHelpers.SubscriptionData(subscriptionId, resultSubscrData[0].Naming,
            resultSubscrData[0].Description, resultSubscrData[0].ProfilesCount, resultSubscrData[0].Price);
        
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "UserSubscriptions/");

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<UserSubscription>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData!.Id);
    }
    
   
    //GET METHOD
    [Fact] public async Task Test02_GetUserSubscriptions_Returns_Status_OK_UserSubscription_For_Current_User()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "UserSubscriptions/");

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<UserSubscription>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
    }
    
    //DELETE METHOD
   [Fact]
   public async Task Test03_DeleteUserSubscription_Returns_Status_OK_In_Case_Of_Success()
   {
       var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
       var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

       var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

       var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
       apiRequest.RequestUri = new Uri(ApiUrl + "UserSubscriptions/");

       var apiResponse = await _client.SendAsync(apiRequest);
       apiResponse.EnsureSuccessStatusCode();
        
       var apiContent = await apiResponse.Content.ReadAsStringAsync();
       var resultData = JsonSerializer.Deserialize<UserSubscription>(apiContent,
           new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

       var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
       newApiRequest.RequestUri = new Uri(ApiUrl + "UserSubscriptions/" + resultData!.Id);
       var newApiResponse = await _client.SendAsync(newApiRequest);
       newApiResponse.EnsureSuccessStatusCode();
   }
}