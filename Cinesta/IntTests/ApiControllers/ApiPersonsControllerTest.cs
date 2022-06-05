using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiPersonsControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private readonly HttpClient _client;

    public ApiPersonsControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiPersonsControllerIntTestingScenario()
    {
        await Test01_PostPerson_Returns_Status_OK_And_PersonId();
        await Test02_GetPersons_Returns_Status_OK_And_IEnumerable_Of_Person();
        await Test03_GetPerson_Returns_Status_OK_And_Person_If_Id_Is_Right();
        await Test04_GetPerson_Returns_Status_NotFound_When_Wrong_Id();
        await Test05_PutPerson_Returns_Status_OK_In_Case_Of_Success();
        await Test06_PutPerson_Returns_Status_BadRequest_If_Id_And_PersonId_Different();
        await Test07_PutPerson_Returns_Status_BadRequest_If_Id_Not_In_Database();
        await Test08_DeletePerson_Returns_Status_OK_In_Case_Of_Success();
    }

    //POST METHOD
    [Fact]
    public async Task Test01_PostPerson_Returns_Status_OK_And_PersonId()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var data = IntTestsHelpers.PersonData(null, "Test", "Test");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<Person>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }

    //GET METHOD
    [Fact]
    public async Task Test02_GetPersons_Returns_Status_OK_And_IEnumerable_Of_Person()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<Person>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Equal(2, resultData!.Count());
    }

    //GET METHOD
    [Fact]
    public async Task Test03_GetPerson_Returns_Status_OK_And_Person_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<Person>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var personId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Persons/" + personId);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();

        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<Person>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Equal(personId, newResultData!.Id);
    }

    //GET METHOD
    [Fact]
    public async Task Test04_GetPerson_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var personId = Guid.NewGuid();

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Persons/" + personId);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    //PUT METHOD
    [Fact]
    public async Task Test05_PutPerson_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<Person>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var personId = resultData![0].Id;

        var editedPerson = IntTestsHelpers.PersonData(personId, "Test2", "Test2");

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Persons/" + personId);
        newApiRequest.Content = editedPerson;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }

    //PUT METHOD
    [Fact]
    public async Task Test06_PutPerson_Returns_Status_BadRequest_If_Id_And_PersonId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Persons");

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<Person>>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var personId = resultData![0].Id;

        var editedPerson = IntTestsHelpers.PersonData(Guid.NewGuid(), "Test2", "Test2");

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Persons/" + personId);
        newApiRequest.Content = editedPerson;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }

    //PUT METHOD
    [Fact]
    public async Task Test07_PutPerson_Returns_Status_BadRequest_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var personId = Guid.NewGuid();

        var editedPerson = IntTestsHelpers.PersonData(personId, "Test2", "Test2");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "Persons/" + personId);
        apiRequest.Content = editedPerson;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }

    //DELETE METHOD
    [Fact]
    public async Task Test08_DeletePerson_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var data = IntTestsHelpers.PersonData(null, "Test", "Test");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "Persons/");

        var apiResponse = await _client.SendAsync(apiRequest);
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<Person>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "Persons/" + resultData!.Id);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}