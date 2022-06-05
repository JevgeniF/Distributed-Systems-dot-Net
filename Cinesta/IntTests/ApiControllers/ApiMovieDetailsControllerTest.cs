using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiMovieDetailsControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private readonly HttpClient _client;
    private const string Culture = "?culture=en-GB";

    public ApiMovieDetailsControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiMovieDetailsControllerIntTestingScenario()
    {
        await Test01_PostMovieDetails_Returns_Status_OK_And_MovieDetailsId();
        await Test02_GetMovieDetails_Returns_Status_OK_And_IEnumerable_Of_MovieDetails_with_Age_Rating_And_Movie_Type();
        await Test03_GetMovieDetails_Returns_Status_OK_And_MovieDetails_with_Age_Rating_And_Movie_Type_If_Id_Is_Right();
        await Test04_GetMovieDetails_Returns_Status_NotFound_When_Wrong_Id();
        await Test05_PutMovieDetails_Returns_Status_OK_In_Case_Of_Success();
        await Test06_PutMovieDetails_Returns_Status_BadRequest_If_Id_And_MovieDetailsId_Different();
        await Test07_PutMovieDetails_Returns_Status_BadRequest_If_Id_Not_In_Database();
        await Test08_DeleteMovieDetails_Returns_Status_OK_In_Case_Of_Success();
    }

    //POST METHOD
    [Fact]
    public async Task Test01_PostMovieDetails_Returns_Status_OK_And_MovieDetailsId()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        //age rating id request
        var apiAgeRatingRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiAgeRatingRequest.RequestUri = new Uri(ApiUrl + "ageRatings/");

        var apiAgeRatingResponse = await _client.SendAsync(apiAgeRatingRequest);

        var apiAgeRatingContent = await apiAgeRatingResponse.Content.ReadAsStringAsync();
        var resultAgeRatingData = JsonSerializer.Deserialize<List<AgeRating>>(apiAgeRatingContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var ageRatingId = resultAgeRatingData![0].Id;
        
        //movie type id request
        var apiMovieTypeRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieTypeRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiMovieTypeResponse = await _client.SendAsync(apiMovieTypeRequest);

        var apiMovieTypeContent = await apiMovieTypeResponse.Content.ReadAsStringAsync();
        var resultApiMovieData = JsonSerializer.Deserialize<List<MovieType>>(apiMovieTypeContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieTypeId = resultApiMovieData![0].Id;
        
        //movieDetailsData
        var data = IntTestsHelpers.MovieDetailsData(null, "posterUri", "Title", DateTime.Now, 
            "Description", ageRatingId, null, movieTypeId, null );

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<MovieDetails>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }

    //GET METHOD
    [Fact]
    public async Task Test02_GetMovieDetails_Returns_Status_OK_And_IEnumerable_Of_MovieDetails_with_Age_Rating_And_Movie_Type()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<MovieDetails>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData!);
        Assert.NotNull(resultData.First().AgeRating);
        Assert.NotNull(resultData.First().MovieType);
    }

    //GET METHOD
    [Fact]
    public async Task Test03_GetMovieDetails_Returns_Status_OK_And_MovieDetails_with_Age_Rating_And_Movie_Type_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieDetails>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + movieDetailsId + Culture);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();

        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<MovieDetails>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(newResultData);
        Assert.Equal(movieDetailsId, newResultData!.Id);
        Assert.NotNull(newResultData.AgeRating);
        Assert.NotNull(newResultData.MovieType);
    }

    //GET METHOD
    [Fact]
    public async Task Test04_GetMovieDetails_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var movieDetailsId = Guid.NewGuid();

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + movieDetailsId + Culture);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }
    
    //PUT METHOD
    [Fact]
    public async Task Test05_PutMovieDetails_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        //age rating id request
        var apiAgeRatingRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiAgeRatingRequest.RequestUri = new Uri(ApiUrl + "ageRatings/");

        var apiAgeRatingResponse = await _client.SendAsync(apiAgeRatingRequest);

        var apiAgeRatingContent = await apiAgeRatingResponse.Content.ReadAsStringAsync();
        var resultAgeRatingData = JsonSerializer.Deserialize<List<AgeRating>>(apiAgeRatingContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var ageRatingId = resultAgeRatingData![0].Id;
        
        //movie type id request
        var apiMovieTypeRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieTypeRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiMovieTypeResponse = await _client.SendAsync(apiMovieTypeRequest);

        var apiMovieTypeContent = await apiMovieTypeResponse.Content.ReadAsStringAsync();
        var resultApiMovieData = JsonSerializer.Deserialize<List<MovieType>>(apiMovieTypeContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieTypeId = resultApiMovieData![0].Id;

        //movie details id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieDetails>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultData![0].Id;

        var editedMovieDetails = IntTestsHelpers.MovieDetailsData(movieDetailsId, "posterUri2",
            "Title2", DateTime.Now, "Description2", ageRatingId, null, movieTypeId, null);

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + movieDetailsId + Culture);
        newApiRequest.Content = editedMovieDetails;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }

    //PUT METHOD
    [Fact]
    public async Task Test06_PutMovieDetails_Returns_Status_BadRequest_If_Id_And_MovieDetailsId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //age rating id request
        var apiAgeRatingRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiAgeRatingRequest.RequestUri = new Uri(ApiUrl + "ageRatings/");

        var apiAgeRatingResponse = await _client.SendAsync(apiAgeRatingRequest);

        var apiAgeRatingContent = await apiAgeRatingResponse.Content.ReadAsStringAsync();
        var resultAgeRatingData = JsonSerializer.Deserialize<List<AgeRating>>(apiAgeRatingContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var ageRatingId = resultAgeRatingData![0].Id;
        
        //movie type id request
        var apiMovieTypeRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieTypeRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiMovieTypeResponse = await _client.SendAsync(apiMovieTypeRequest);

        var apiMovieTypeContent = await apiMovieTypeResponse.Content.ReadAsStringAsync();
        var resultApiMovieData = JsonSerializer.Deserialize<List<MovieType>>(apiMovieTypeContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieTypeId = resultApiMovieData![0].Id;

        //movie details id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieDetails>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultData![0].Id;

        var editedMovieDetails = IntTestsHelpers.MovieDetailsData(Guid.NewGuid(), "posterUri2",
            "Title2", DateTime.Now, "Description2", ageRatingId, null, movieTypeId, null);

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + movieDetailsId + Culture);
        newApiRequest.Content = editedMovieDetails;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }
    
    //PUT METHOD
    [Fact]
    public async Task Test07_PutMovieDetails_Returns_Status_BadRequest_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var movieDetailsId = Guid.NewGuid();

        var editedMovieDetails = IntTestsHelpers.MovieDetailsData(movieDetailsId, "posterUri2",
            "Title2", DateTime.Now, "Description2", Guid.NewGuid(), null,
            Guid.NewGuid(), null);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + movieDetailsId  + Culture);
        apiRequest.Content = editedMovieDetails;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }
    
    //DELETE METHOD
    [Fact]
    public async Task Test08_DeleteMovieDetails_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //age rating id request
        var apiAgeRatingRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiAgeRatingRequest.RequestUri = new Uri(ApiUrl + "ageRatings/");

        var apiAgeRatingResponse = await _client.SendAsync(apiAgeRatingRequest);

        var apiAgeRatingContent = await apiAgeRatingResponse.Content.ReadAsStringAsync();
        var resultAgeRatingData = JsonSerializer.Deserialize<List<AgeRating>>(apiAgeRatingContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var ageRatingId = resultAgeRatingData![0].Id;
        
        //movie type id request
        var apiMovieTypeRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieTypeRequest.RequestUri = new Uri(ApiUrl + "MovieTypes" + Culture);

        var apiMovieTypeResponse = await _client.SendAsync(apiMovieTypeRequest);

        var apiMovieTypeContent = await apiMovieTypeResponse.Content.ReadAsStringAsync();
        var resultApiMovieData = JsonSerializer.Deserialize<List<MovieType>>(apiMovieTypeContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieTypeId = resultApiMovieData![0].Id;
        
        //movieDetailsData
        var data = IntTestsHelpers.MovieDetailsData(null, "posterUri", "Title", DateTime.Now, 
            "Description", ageRatingId, null, movieTypeId, null );

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<MovieDetails>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/" + resultData!.Id + Culture);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}