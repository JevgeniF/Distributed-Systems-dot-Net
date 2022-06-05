using System.Net;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests.ApiControllers;

public class ApiCastRoleControllerTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";
    private const string Culture = "?culture=en-GB";

    public ApiCastRoleControllerTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiCastRolesControllerIntTestingScenario()
    {
        await Test01_PostCastRole_Returns_Status_OK_And_CastRoleId();
        await Test02_GetCastRoles_Returns_Status_OK_And_IEnumerable_Of_CastRole();
        await Test03_GetCastRole_Returns_Status_OK_And_CastRole_If_Id_Is_Right();
        await Test04_GetCastRole_Returns_Status_NotFound_When_Wrong_Id();
        await Test05_PutCastRole_Returns_Status_OK_In_Case_Of_Success();
        await Test06_PutCastRole_Returns_Status_BadRequest_If_Id_And_CastRoleId_Different();
        await Test07_PutCastRole_Returns_Status_NotFound_If_Id_Not_In_Database();
        await Test08_DeleteCastRole_Returns_Status_OK_In_Case_Of_Success();
    }
    
    //POST METHOD
    [Fact]
    public async Task Test01_PostCastRole_Returns_Status_OK_And_CastRoleId()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var data = IntTestsHelpers.CastRoleData(null,"Test");
        
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = IntTestsHelpers.ResultData<CastRole>(apiContent);
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData.Id);
    }

    //GET METHOD
    [Fact] public async Task Test02_GetCastRoles_Returns_Status_OK_And_IEnumerable_Of_CastRole()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<IEnumerable<CastRole>>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Single(resultData);
    }
    
    //GET METHOD
    [Fact] public async Task Test03_GetCastRole_Returns_Status_OK_And_CastRole_If_Id_Is_Right()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<CastRole>>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castRoleId = resultData![0].Id;

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "castRoles/" + castRoleId + Culture);
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        apiContent = await apiResponse.Content.ReadAsStringAsync();
        var newResultData = JsonSerializer.Deserialize<CastRole>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Equal(castRoleId, newResultData!.Id);
    }
    
    //GET METHOD
    [Fact] public async Task Test04_GetCastRole_Returns_Status_NotFound_When_Wrong_Id()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var castRoleId = Guid.NewGuid();

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "castRoles/" + castRoleId + Culture);
        var apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }
    
    //PUT METHOD
    [Fact] public async Task Test05_PutCastRole_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<CastRole>>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castRoleId = resultData![0].Id;

        var editedCastRole = IntTestsHelpers.CastRoleData(castRoleId,"Test2");

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "castRoles/" + castRoleId + Culture);
        newApiRequest.Content = editedCastRole;
        apiResponse = await _client.SendAsync(newApiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }
    
    //PUT METHOD
    [Fact] public async Task Test06_PutCastRole_Returns_Status_BadRequest_If_Id_And_CastRoleId_Different()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Get, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "castRoles" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<CastRole>>(apiContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var castRoleId = resultData![0].Id;

        var editedCastRole = IntTestsHelpers.CastRoleData(Guid.NewGuid(),"Test2");

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "castRoles/" + castRoleId + Culture);
        newApiRequest.Content = editedCastRole;
        apiResponse = await _client.SendAsync(newApiRequest);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
    }
    
    //PUT METHOD
    [Fact] public async Task Test07_PutCastRole_Returns_Status_NotFound_If_Id_Not_In_Database()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);
        
        var castRoleId = Guid.NewGuid();

        var editedCastRole = IntTestsHelpers.CastRoleData(castRoleId,"Test2");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Put, resultJWT!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "castRoles/" + castRoleId + Culture);
        apiRequest.Content = editedCastRole;
        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.NotFound, apiResponse.StatusCode);
    }
    
    //DELETE METHOD
    [Fact]
    public async Task Test08_DeleteCastRole_Returns_Status_OK_In_Case_Of_Success()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJWT = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var data = IntTestsHelpers.CastRoleData(null,"Test");
        
        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJWT!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "castRoles/" + Culture);

        var apiResponse = await _client.SendAsync(apiRequest);
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = System.Text.Json.JsonSerializer.Deserialize<CastRole>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var newApiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Delete, resultJWT.Token);
        newApiRequest.RequestUri = new Uri(ApiUrl + "castRoles/" + resultData!.Id + Culture);
        var newApiResponse = await _client.SendAsync(newApiRequest);
        newApiResponse.EnsureSuccessStatusCode();
    }
}