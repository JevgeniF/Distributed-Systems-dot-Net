using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiProfileFavoriteMoviesControllerTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private const string Culture = "?culture=en-GB";

    public ApiProfileFavoriteMoviesControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiProfileFavoriteMoviesControllerIntTestingScenario()
    {
        await Test01_PostProfileFavoriteMovie_Returns_Status_OK_And_ProfileFavoriteMovieId();
        await Test02_GetProfileFavoriteMovies_Returns_Status_OK_And_IEnumerable_Of_ProfileFavoriteMovies();
        await Test03_DeleteProfileFavoriteMovie_Returns_Status_OK_In_Case_Of_Success();
    }
    
    //POST METHOD
    [Fact]
    public async Task Test01_PostProfileFavoriteMovie_Returns_Status_OK_And_ProfileFavoriteMovieId()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
    
        //profile id request
        var apiProfileRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiProfileRequest.RequestUri = new Uri(ApiUrl + "UserProfiles/");

        var apiProfileResponse = await _client.SendAsync(apiProfileRequest);
        apiProfileResponse.EnsureSuccessStatusCode();
        
        var apiProfileContent = await apiProfileResponse.Content.ReadAsStringAsync();
        var resultProfileData = JsonSerializer.Deserialize<IEnumerable<UserProfile>>(apiProfileContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        
        //movie details id request
        var apiMovieRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieResponse = await _client.SendAsync(apiMovieRequest);

        var apiMovieContent = await apiMovieResponse.Content.ReadAsStringAsync();
        var resultMovieData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieData![0].Id;

        var data = IntTestsHelpers.ProfileFavoriteMovieData(null,
            resultProfileData!.First().Id, null, movieDetailsId, null);
        
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "ProfileFavoriteMovies/");

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<ProfileFavoriteMovie>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData!.Id);
    }
    
    
    //GET METHOD
    [Fact] public async Task Test02_GetProfileFavoriteMovies_Returns_Status_OK_And_IEnumerable_Of_ProfileFavoriteMovies()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        //profile id request
        var apiProfileRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiProfileRequest.RequestUri = new Uri(ApiUrl + "UserProfiles/");

        var apiProfileResponse = await _client.SendAsync(apiProfileRequest);
        apiProfileResponse.EnsureSuccessStatusCode();
        
        var apiProfileContent = await apiProfileResponse.Content.ReadAsStringAsync();
        var resultProfileData = JsonSerializer.Deserialize<IEnumerable<UserProfile>>(apiProfileContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "ProfileFavoriteMovies/" + resultProfileData!.First().Id);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<ProfileFavoriteMovie>>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData);
    }

    //DELETE METHOD
    [Fact]
    public async Task Test03_DeleteProfileFavoriteMovie_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //profile id request
        var apiProfileRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiProfileRequest.RequestUri = new Uri(ApiUrl + "UserProfiles/");

        var apiProfileResponse = await _client.SendAsync(apiProfileRequest);
        apiProfileResponse.EnsureSuccessStatusCode();
        
        var apiProfileContent = await apiProfileResponse.Content.ReadAsStringAsync();
        var resultProfileData = JsonSerializer.Deserialize<IEnumerable<UserProfile>>(apiProfileContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "ProfileFavoriteMovies/" + resultProfileData!.First().Id);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<ProfileFavoriteMovie>>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        
        
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "ProfileFavoriteMovies/" + resultData!.First().Id);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}