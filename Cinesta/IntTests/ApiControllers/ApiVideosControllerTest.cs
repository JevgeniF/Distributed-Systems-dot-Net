using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiVideosControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private readonly HttpClient _client;
    private const string Culture = "?culture=en-GB";

    public ApiVideosControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiVideoControllerIntTestingScenario()
    {
        await Test01_PostVideo_Returns_Status_OK_And_VideoId();
        await Test02_GetVideos_Returns_Status_OK_And_IEnumerable_Of_Video_with_MovieDetails();
        await Test03_GetVideo_Returns_Status_OK_And_Video_If_Id_Is_Right();
        await Test04_GetVideo_Returns_Status_NotFound_When_Wrong_Id();
        await Test05_PutVideo_Returns_Status_OK_In_Case_Of_Success();
        await Test06_PutVideo_Returns_Status_BadRequest_If_Id_And_VideoId_Different();
        await Test07_PutVideo_Returns_Status_BadRequest_If_Id_Not_In_Database();
        await Test08_DeleteVideo_Returns_Status_OK_In_Case_Of_Success();
    }

    //POST METHOD
    [Fact]
    public async Task Test01_PostVideo_Returns_Status_OK_And_VideoId()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails" + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultApiMovieDetails = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultApiMovieDetails![0].Id;
        
        //VideoData
        var data = IntTestsHelpers.VideoData(null, 0, "Title", "FileUri", 
            "Description", movieDetailsId, null );

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "Videos" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<Video>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }

    //GET METHOD
    [Fact]
    public async Task Test02_GetVideos_Returns_Status_OK_And_IEnumerable_Of_Video_with_MovieDetails()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Videos" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<Video>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData!);
        Assert.NotNull(resultData.First().MovieDetails);
    }

    //GET METHOD
    [Fact]
    public async Task Test03_GetVideo_Returns_Status_OK_And_Video_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Videos" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<Video>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var videoId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Videos/" + videoId + Culture);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();

        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<Video>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(newResultData);
        Assert.Equal(videoId, newResultData!.Id);
        Assert.NotNull(newResultData.MovieDetailsId);
    }

    //GET METHOD
    [Fact]
    public async Task Test04_GetVideo_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var videoId = Guid.NewGuid();

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Video/" + videoId + Culture);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    //PUT METHOD
    [Fact]
    public async Task Test05_PutVideo_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails" + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultApiMovieDetails = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultApiMovieDetails![0].Id;

        //video id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Videos/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<Video>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var videoId = resultData![0].Id;

        var editedVideo = IntTestsHelpers.VideoData(videoId, 1, "Title1", "FileUri1", 
            "Description1", movieDetailsId, null);

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Videos/" + videoId + Culture);
        newApiRequest.Content = editedVideo;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }
    
    //PUT METHOD
    [Fact]
    public async Task Test06_PutVideo_Returns_Status_BadRequest_If_Id_And_VideoId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails" + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultApiMovieDetails = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultApiMovieDetails![0].Id;

        //video id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Videos/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<Video>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var videoId = resultData![0].Id;

        var editedVideo = IntTestsHelpers.VideoData(Guid.NewGuid(), 1, "Title1", "FileUri1", 
            "Description1", movieDetailsId, null);
        
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Videos/" + videoId + Culture);
        newApiRequest.Content = editedVideo;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }
    
    //PUT METHOD
    [Fact]
    public async Task Test07_PutVideo_Returns_Status_BadRequest_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        var videoId = Guid.NewGuid();

        var editedVideo = IntTestsHelpers.VideoData(videoId, 1, "Title1", "FileUri1", 
            "Description1", Guid.NewGuid(), null);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Videos/" + videoId  + Culture);
        apiRequest.Content = editedVideo;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }
    
    //DELETE METHOD
    [Fact]
    public async Task Test08_DeleteVideo_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails" + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultApiMovieDetails = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultApiMovieDetails![0].Id;
        
        //VideoData
        var data = IntTestsHelpers.VideoData(null, 0, "Title", "FileUri", 
            "Description", movieDetailsId, null );

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "Videos/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<Video>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Videos/" + resultData!.Id + Culture);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}