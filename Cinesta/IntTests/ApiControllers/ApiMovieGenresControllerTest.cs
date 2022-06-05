using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiMovieGenresControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private readonly HttpClient _client;
    private const string Culture = "?culture=en-GB";

    public ApiMovieGenresControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiMovieGenreControllerIntTestingScenario()
    {
        await Test01_PostMovieGenre_Returns_Status_OK_And_MovieGenreId();
        await Test02_GetMovieGenres_Returns_Status_OK_And_IEnumerable_Of_MovieGenre_with_MovieDetails_And_Genre();
        await Test03_GetMovieGenre_Returns_Status_OK_And_MovieGenre_If_Id_Is_Right();
        await Test04_GetMovieGenre_Returns_Status_NotFound_When_Wrong_Id();
        await Test05_PutMovieGenre_Returns_Status_OK_In_Case_Of_Success();
        await Test06_PutMovieGenre_Returns_Status_BadRequest_If_Id_And_MovieGenreId_Different();
        await Test07_PutMovieGenre_Returns_Status_BadRequest_If_Id_Not_In_Database();
        await Test08_DeleteMovieGenre_Returns_Status_OK_In_Case_Of_Success();
    }

    //POST METHOD
    [Fact]
    public async Task Test01_PostMovieGenre_Returns_Status_OK_And_MovieGenreId()
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
        
        //genre id request
        var apiGenreRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiGenreRequest.RequestUri = new Uri(ApiUrl + "Genres" + Culture);

        var apiGenreResponse = await _client.SendAsync(apiGenreRequest);

        var apiGenreContent = await apiGenreResponse.Content.ReadAsStringAsync();
        var resultGenreData = JsonSerializer.Deserialize<List<Genre>>(apiGenreContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var genreId = resultGenreData![0].Id;
        
        //MovieGenreData
        var data = IntTestsHelpers.MovieGenreData(null, movieDetailsId, null, genreId, null );

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<MovieGenre>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }

    
    //GET METHOD
    [Fact]
    public async Task Test02_GetMovieGenres_Returns_Status_OK_And_IEnumerable_Of_MovieGenre_with_MovieDetails_And_Genre()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<MovieGenre>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData!);
        Assert.NotNull(resultData.First().MovieDetails!.Title);
        Assert.NotNull(resultData.First().Genre!.Naming);
    }

    
    //GET METHOD
    [Fact]
    public async Task Test03_GetMovieGenre_Returns_Status_OK_And_MovieGenre_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieGenre>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieGenreId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres/" + movieGenreId + Culture);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();

        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<MovieGenre>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(newResultData);
        Assert.Equal(movieGenreId, newResultData!.Id);
    }

    //GET METHOD
    [Fact]
    public async Task Test04_GetMovieGenre_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var movieGenreId = Guid.NewGuid();

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieGenre/" + movieGenreId + Culture);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    
    //PUT METHOD
    [Fact]
    public async Task Test05_PutMovieGenre_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        //creating one more genre for change
        var data = IntTestsHelpers.GenreData(null, "TestGenreForMovieGenre");

        var apiGenreRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiGenreRequest.Content = data;
        apiGenreRequest.RequestUri = new Uri(ApiUrl + "Genres" + Culture);

        var apiGenreResponse = await _client.SendAsync(apiGenreRequest);
        apiGenreResponse.EnsureSuccessStatusCode();

        var apiGenreContent = await apiGenreResponse.Content.ReadAsStringAsync();
        var resultGenreData = IntTestsHelpers.ResultData<Genre>(apiGenreContent);
        var newGenreId = resultGenreData.Id;
        
        //movie details id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails" + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultApiMovieDetails = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultApiMovieDetails![0].Id;

        //MovieGenre id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieGenre>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieGenreId = resultData![0].Id;

        var editedMovieGenre = IntTestsHelpers.MovieGenreData(movieGenreId,
            movieDetailsId,
            null, newGenreId, null);

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres/" + movieGenreId + Culture);
        newApiRequest.Content = editedMovieGenre;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }
    
    
    //PUT METHOD
    [Fact]
    public async Task Test06_PutMovieGenre_Returns_Status_BadRequest_If_Id_And_MovieGenreId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //MovieGenre id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<MovieGenre>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieGenreId = resultData![0].Id;

        var editedMovieGenre = IntTestsHelpers.MovieGenreData(Guid.NewGuid(), Guid.NewGuid(),
            null, Guid.NewGuid(), null);
        
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres/" + movieGenreId + Culture);
        newApiRequest.Content = editedMovieGenre;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }
    
    //PUT METHOD
    [Fact]
    public async Task Test07_PutMovieGenre_Returns_Status_BadRequest_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        var movieGenreId = Guid.NewGuid();

        var editedMovieGenre = IntTestsHelpers.MovieGenreData(movieGenreId, Guid.NewGuid(), 
            null, Guid.NewGuid(), null);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres/" + movieGenreId  + Culture);
        apiRequest.Content = editedMovieGenre;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    //DELETE METHOD
    [Fact]
    public async Task Test08_DeleteMovieGenre_Returns_Status_OK_In_Case_Of_Success()
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
        
        //genre id request
        var apiGenreRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiGenreRequest.RequestUri = new Uri(ApiUrl + "Genres" + Culture);

        var apiGenreResponse = await _client.SendAsync(apiGenreRequest);

        var apiGenreContent = await apiGenreResponse.Content.ReadAsStringAsync();
        var resultGenreData = JsonSerializer.Deserialize<List<Genre>>(apiGenreContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var genreId = resultGenreData![0].Id;
        
        //MovieGenreData
        var data = IntTestsHelpers.MovieGenreData(null, movieDetailsId, null, genreId, null );

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<MovieGenre>(apiContent);
        
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "MovieGenres/" + resultData.Id + Culture);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}