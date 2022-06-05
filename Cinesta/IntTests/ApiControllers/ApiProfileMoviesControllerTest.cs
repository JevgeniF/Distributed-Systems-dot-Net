using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiProfileMoviesControllerTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private const string Culture = "?culture=en-GB";

    public ApiProfileMoviesControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiProfileMoviesControllerIntTestingScenario()
    {
        await Test01_GetProfileMovies_Returns_Status_OK_And_Movies_For_Profile();
    }

    //GET METHOD
    [Fact] public async Task Test01_GetProfileMovies_Returns_Status_OK_And_Movies_For_Profile()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        var apiProfileRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiProfileRequest.RequestUri = new Uri(ApiUrl + "UserProfiles/");

        var apiProfileResponse = await _client.SendAsync(apiProfileRequest);
        apiProfileResponse.EnsureSuccessStatusCode();
        
        var apiProfileContent = await apiProfileResponse.Content.ReadAsStringAsync();
        var resultProfileData = JsonSerializer.Deserialize<IEnumerable<UserProfile>>(apiProfileContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "ProfileMovies/" + resultProfileData!.First().Id);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<ProfileMovie>>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
    }
}