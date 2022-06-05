#pragma warning disable xUnit2002
using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiCastInMoviesControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private readonly HttpClient _client;
    private const string Culture = "?culture=en-GB";

    public ApiCastInMoviesControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiCastInMovieControllerIntTestingScenario()
    {
        await Test01_PostCastInMovie_Returns_Status_OK_And_CastInMovieId();
        await Test02_GetCastInMovies_Returns_Status_OK_And_IEnumerable_Of_CastInMovie_with_Connected_Objects();
        await Test03_GetCastInMovie_Returns_Status_OK_And_IEnumerable_Of_CastInMovie_Found_By_MovieId();
        await Test04_GetCastInMovie_Returns_Status_OK_And_CastInMovie_If_Id_Is_Right();
        await Test05_GetCastInMovie_Returns_Status_NotFound_When_Wrong_Id();
        await Test06_PutCastInMovie_Returns_Status_OK_In_Case_Of_Success();
        await Test07_PutCastInMovie_Returns_Status_BadRequest_If_Id_And_CastInMovieId_Different();
        await Test08_PutCastInMovie_Returns_Status_BadRequest_If_Id_Not_In_Database();
        await Test09_DeleteCastInMovie_Returns_Status_OK_In_Case_Of_Success();
    }

    //POST METHOD
    [Fact]
    public async Task Test01_PostCastInMovie_Returns_Status_OK_And_CastInMovieId()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //cast role id request
        var apiCastRoleRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiCastRoleRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiCastRoleResponse = await _client.SendAsync(apiCastRoleRequest);

        var apiCastRoleContent = await apiCastRoleResponse.Content.ReadAsStringAsync();
        var resultCastRoleData = JsonSerializer.Deserialize<List<CastRole>>(apiCastRoleContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castRoleId = resultCastRoleData![0].Id;
        
        //genre id request
        var apiPersonRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiPersonRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiPersonResponse = await _client.SendAsync(apiPersonRequest);

        var apiPersonContent = await apiPersonResponse.Content.ReadAsStringAsync();
        var resultPersonData = JsonSerializer.Deserialize<List<Person>>(apiPersonContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var personId = resultPersonData![0].Id;
        
        //movie id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultMovieDetailsData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieDetailsData![0].Id;
        
        //CastInMovieData
        var data = IntTestsHelpers.CastInMovieData(null, castRoleId, null, personId, null,
            movieDetailsId, null);

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<CastInMovie>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }

    
    //GET METHOD
    [Fact]
    public async Task Test02_GetCastInMovies_Returns_Status_OK_And_IEnumerable_Of_CastInMovie_with_Connected_Objects()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<CastInMovie>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData!);
        Assert.NotNull(resultData.First().MovieDetailsId);
        Assert.NotNull(resultData.First().PersonId);
        Assert.NotNull(resultData.First().CastRoleId);
    }

    //GET METHOD
    [Fact]
    public async Task Test03_GetCastInMovie_Returns_Status_OK_And_IEnumerable_Of_CastInMovie_Found_By_MovieId()
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
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/movie=" + movieDetailsId + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<CastInMovie>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData!);
        Assert.NotNull(resultData.First().MovieDetailsId);
        Assert.NotNull(resultData.First().PersonId);
        Assert.NotNull(resultData.First().CastRoleId);
    }
    
    
    
    //GET METHOD
    [Fact]
    public async Task Test04_GetCastInMovie_Returns_Status_OK_And_CastInMovie_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<CastInMovie>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castInMovieId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/" + castInMovieId + Culture);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();

        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<CastInMovie>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(newResultData);
        Assert.Equal(castInMovieId, newResultData!.Id);
    }
    

    //GET METHOD
    [Fact]
    public async Task Test05_GetCastInMovie_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var castInMovieId = Guid.NewGuid();

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "CastInMovie/" + castInMovieId + Culture);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    
    //PUT METHOD
    [Fact]
    public async Task Test06_PutCastInMovie_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        
        //cast role id request
        var apiCastRoleRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiCastRoleRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiCastRoleResponse = await _client.SendAsync(apiCastRoleRequest);

        var apiCastRoleContent = await apiCastRoleResponse.Content.ReadAsStringAsync();
        var resultCastRoleData = JsonSerializer.Deserialize<List<CastRole>>(apiCastRoleContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castRoleId = resultCastRoleData![0].Id;
        
        //genre id request
        var apiPersonRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiPersonRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiPersonResponse = await _client.SendAsync(apiPersonRequest);

        var apiPersonContent = await apiPersonResponse.Content.ReadAsStringAsync();
        var resultPersonData = JsonSerializer.Deserialize<List<Person>>(apiPersonContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var personId = resultPersonData![0].Id;
        
        //movie id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultMovieDetailsData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieDetailsData![0].Id;

        //CastInMovie id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<CastInMovie>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castInMovieId = resultData![0].Id;

        var editedCastInMovie = IntTestsHelpers.CastInMovieData(castInMovieId,
            castRoleId,
            null, personId, null, movieDetailsId, null);

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/" + castInMovieId + Culture);
        newApiRequest.Content = editedCastInMovie;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }
    
    
    //PUT METHOD
    [Fact]
    public async Task Test07_PutCastInMovie_Returns_Status_BadRequest_If_Id_And_CastInMovieId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //CastInMovie id request
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/"  + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<CastInMovie>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castInMovieId = resultData![0].Id;

        var editedCastInMovie = IntTestsHelpers.CastInMovieData(Guid.NewGuid(), Guid.NewGuid(),
            null, Guid.NewGuid(), null, Guid.NewGuid(), null);
        
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/" + castInMovieId + Culture);
        newApiRequest.Content = editedCastInMovie;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }
    
    //PUT METHOD
    [Fact]
    public async Task Test08_PutCastInMovie_Returns_Status_BadRequest_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        var castInMovieId = Guid.NewGuid();

        var editedCastInMovie = IntTestsHelpers.CastInMovieData(castInMovieId, Guid.NewGuid(),
            null, Guid.NewGuid(), null, Guid.NewGuid(), null);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/" + castInMovieId  + Culture);
        apiRequest.Content = editedCastInMovie;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }
    
    //DELETE METHOD
    [Fact]
    public async Task Test09_DeleteCastInMovie_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        //cast role id request
        var apiCastRoleRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiCastRoleRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiCastRoleResponse = await _client.SendAsync(apiCastRoleRequest);

        var apiCastRoleContent = await apiCastRoleResponse.Content.ReadAsStringAsync();
        var resultCastRoleData = JsonSerializer.Deserialize<List<CastRole>>(apiCastRoleContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castRoleId = resultCastRoleData![0].Id;
        
        //genre id request
        var apiPersonRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiPersonRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiPersonResponse = await _client.SendAsync(apiPersonRequest);

        var apiPersonContent = await apiPersonResponse.Content.ReadAsStringAsync();
        var resultPersonData = JsonSerializer.Deserialize<List<Person>>(apiPersonContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var personId = resultPersonData![0].Id;
        
        //movie id request
        var apiMovieDetailsRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiMovieDetailsRequest.RequestUri = new Uri(ApiUrl + "MovieDetails/"  + Culture);

        var apiMovieDetailsResponse = await _client.SendAsync(apiMovieDetailsRequest);

        var apiMovieDetailsContent = await apiMovieDetailsResponse.Content.ReadAsStringAsync();
        var resultMovieDetailsData = JsonSerializer.Deserialize<List<MovieDetails>>(apiMovieDetailsContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var movieDetailsId = resultMovieDetailsData![0].Id;
        
        //CastInMovieData
        var data = IntTestsHelpers.CastInMovieData(null, castRoleId, null, personId, null,
            movieDetailsId, null);

        //post
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<CastInMovie>(apiContent);
        
        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "CastInMovies/" + resultData.Id + Culture);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}