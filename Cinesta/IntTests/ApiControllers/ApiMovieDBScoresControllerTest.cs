#pragma warning disable xUnit2002
using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiMovieDbScoresControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private readonly HttpClient _client;
    private const string Culture = "?culture=en-GB";

    public ApiMovieDbScoresControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiMovieDbScoreControllerIntTestingScenario()
    {
        await Test01_PostMovieDbScore_Returns_Status_OK_And_MovieDbScoreId();
        await Test02_GetMovieDbScores_Returns_Status_OK_And_IEnumerable_Of_MovieDbScore_with_Connected_Objects();
        await Test03_GetMovieDbScore_Returns_Status_OK_And_IEnumerable_Of_MovieDbScore_Found_By_MovieId();
        await Test04_GetMovieDbScore_Returns_Status_OK_And_MovieDbScore_If_Id_Is_Right();
        await Test05_GetMovieDbScore_Returns_Status_NotFound_When_Wrong_Id();
        await Test06_PutMovieDbScore_Returns_Status_OK_In_Case_Of_Success();
        await Test07_PutMovieDbScore_Returns_Status_BadRequest_If_Id_And_MovieDbScoreId_Different();
        await Test08_PutMovieDbScore_Returns_Status_BadRequest_If_Id_Not_In_Database();
        await Test09_DeleteMovieDbScore_Returns_Status_OK_In_Case_Of_Success();
    }

    //POST METHOD
    [Fact]
    public async Task Test01_PostMovieDbScore_Returns_Status_OK_And_MovieDbScoreId()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultMovieDetailsData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieDetailsData![0].Id;
        
        //MovieDbScoreData
        var data = IntTestsHelpers.MovieDbScoreData(null, "tt000000", 0,
            movieDetailsId, null);

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<MovieDbScore>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }
    
    //GET METHOD
    [Fact]
    public async Task Test02_GetMovieDbScores_Returns_Status_OK_And_IEnumerable_Of_MovieDbScore_with_Connected_Objects()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<MovieDbScore>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData!);
        Assert.NotNull(resultData.First().MovieDetailsId);
    }
    
    //GET METHOD
    [Fact]
    public async Task Test03_GetMovieDbScore_Returns_Status_OK_And_IEnumerable_Of_MovieDbScore_Found_By_MovieId()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        //movie id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultMovieDetailsData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieDetailsData![0].Id;
        
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/movie=" + movieDetailsId + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<MovieDbScore>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.NotNull(resultData!.MovieDetailsId);
    }

    //GET METHOD
    [Fact]
    public async Task Test04_GetMovieDbScore_Returns_Status_OK_And_MovieDbScore_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieDbScore>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDbScoreId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/" + movieDbScoreId + Culture);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();

        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<MovieDbScore>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(newResultData);
        Assert.Equal(movieDbScoreId, newResultData!.Id);
    }
    
    
      
    //GET METHOD
    [Fact]
    public async Task Test05_GetMovieDbScore_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
  
        var movieDbScoreId = Guid.NewGuid();
  
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScore/" + movieDbScoreId + Culture);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }
      
    //PUT METHOD
    [Fact]
    public async Task Test06_PutMovieDbScore_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
              
        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultMovieDetailsData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieDetailsData![0].Id;

        //MovieDbScore id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/"  + Culture);
      
        var apiResponse = await _client.SendAsync(apiRequest);
      
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieDbScore>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDbScoreId = resultData![0].Id;
      
        var editedMovieDbScore = IntTestsHelpers.MovieDbScoreData(movieDbScoreId,
            "tt1111111", 0, movieDetailsId, null);
      
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/" + movieDbScoreId + Culture);
        newApiRequest.Content = editedMovieDbScore;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }
          
    //PUT METHOD
    [Fact]
    public async Task Test07_PutMovieDbScore_Returns_Status_BadRequest_If_Id_And_MovieDbScoreId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
     
        //MovieDbScore id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/"  + Culture);
     
        var apiResponse = await _client.SendAsync(apiRequest);
     
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieDbScore>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDbScoreId = resultData![0].Id;
     
        var editedMovieDbScore = IntTestsHelpers.MovieDbScoreData(Guid.NewGuid(), "tt000000",
            0, Guid.NewGuid(), null);
             
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/" + movieDbScoreId + Culture);
        newApiRequest.Content = editedMovieDbScore;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }
         
//PUT METHOD
    [Fact]
    public async Task Test08_PutMovieDbScore_Returns_Status_BadRequest_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
   
        var movieDbScoreId = Guid.NewGuid();

        var editedMovieDbScore = IntTestsHelpers.MovieDbScoreData(movieDbScoreId, "tt000000",
            0, Guid.NewGuid(), null);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/" + movieDbScoreId  + Culture);
        apiRequest.Content = editedMovieDbScore;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

//DELETE METHOD
    [Fact]
    public async Task Test09_DeleteMovieDbScore_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultMovieDetailsData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieDetailsData![0].Id;
        
        //MovieDbScoreData
        var data = IntTestsHelpers.MovieDbScoreData(null, "tt000000", 0,
            movieDetailsId, null);

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<MovieDbScore>(apiContent);
   
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDbScores/" + resultData.Id + Culture);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}