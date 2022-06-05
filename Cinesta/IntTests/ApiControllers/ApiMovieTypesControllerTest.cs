using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiMovieTypesControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private const string Culture = "?culture=en-GB";
    private readonly HttpClient _client;

    public ApiMovieTypesControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }

    //SCENARIO
    [Fact]
    public async Task ApiMovieTypesControllerIntTestingScenario()
    {
        await Test01_PostMovieType_Returns_Status_OK_And_MovieTypeId();
        await Test02_GetMovieTypes_Returns_Status_OK_And_IEnumerable_Of_MovieType();
        await Test03_GetMovieType_Returns_Status_OK_And_MovieType_If_Id_Is_Right();
        await Test04_GetMovieType_Returns_Status_NotFound_When_Wrong_Id();
        await Test05_PutMovieType_Returns_Status_OK_In_Case_Of_Success();
        await Test06_PutMovieType_Returns_Status_BadRequest_If_Id_And_MovieTypeId_Different();
        await Test07_PutMovieType_Returns_Status_NotFound_If_Id_Not_In_Database();
        await Test08_DeleteMovieType_Returns_Status_OK_In_Case_Of_Success();
    }

    //POST METHOD
    [Fact]
    public async Task Test01_PostMovieType_Returns_Status_OK_And_MovieTypeId()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var data = IntTestsHelpers.MovieTypeData(null, "Test");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<MovieType>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }

    //GET METHOD
    [Fact]
    public async Task Test02_GetMovieTypes_Returns_Status_OK_And_IEnumerable_Of_MovieType()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<MovieType>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData);
    }

    //GET METHOD
    [Fact]
    public async Task Test03_GetMovieType_Returns_Status_OK_And_MovieType_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieType>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieTypeId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes/" + movieTypeId + Culture);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();

        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<MovieType>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Equal(movieTypeId, newResultData!.Id);
    }

    //GET METHOD
    [Fact]
    public async Task Test04_GetMovieType_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var movieTypeId = Guid.NewGuid();

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes/" + movieTypeId + Culture);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    //PUT METHOD
    [Fact]
    public async Task Test05_PutMovieType_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieType>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieTypeId = resultData![0].Id;

        var editedMovieType = IntTestsHelpers.MovieTypeData(movieTypeId, "Test2");

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes/" + movieTypeId + Culture);
        newApiRequest.Content = editedMovieType;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }

    //PUT METHOD
    [Fact]
    public async Task Test06_PutMovieType_Returns_Status_BadRequest_If_Id_And_MovieTypeId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieType>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieTypeId = resultData![0].Id;

        var editedMovieType = IntTestsHelpers.MovieTypeData(Guid.NewGuid(), "Test2");

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes/" + movieTypeId + Culture);
        newApiRequest.Content = editedMovieType;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }

    //PUT METHOD
    [Fact]
    public async Task Test07_PutMovieType_Returns_Status_NotFound_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var movieTypeId = Guid.NewGuid();

        var editedMovieType = IntTestsHelpers.MovieTypeData(movieTypeId, "Test2");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes/" + movieTypeId + Culture);
        apiRequest.Content = editedMovieType;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    //DELETE METHOD
    [Fact]
    public async Task Test08_DeleteMovieType_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var data = IntTestsHelpers.MovieTypeData(null, "Test");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes/" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<MovieType>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieTypes/" + resultData!.Id + Culture);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}