using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace IntTests.ApiControllers;

public class ApiAccountControllerIntTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string ApiUrl = "https://cinesta.azurewebsites.net/api/v1/";

    public ApiAccountControllerIntTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task ApiAccountControllerIntTestingScenario()
    {
        await Test01_Post_Register_Returns_Status_OK_And_JWT_data();
        await Test02_Post_Register_Returns_Status_BadRequest_When_Email_Already_Registered();
        await Test03_Post_Login_Returns_Status_OK_And_JWT_data();
        await Test04_Get_Login_With_Wrong_Data_Returns_Status_NotFound();
        await Test05_Post_RefreshToken_Returns_Status_OK_And_New_RefreshToken();
        await Test06_Get_UsersList_Returns_List_Of_Users_For_Admin();
        await Test07_Get_UsersList_Returns_Forbidden_For_User();
        await Test08_Post_ChangeRole_Returns_Forbidden_For_User();
        await Test09_Post_ChangeRole_Returns_OK_For_Admin();
    }

    // REGISTER METHOD
    [Fact]
    public async Task Test01_Post_Register_Returns_Status_OK_And_JWT_data()
    {
        var registerData = IntTestsHelpers.RegisterData("Test", "User", "user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/register", registerData);
        response.EnsureSuccessStatusCode();
        
        var requestContent = await response.Content.ReadAsStringAsync();
        var resultJwt = JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );
        Assert.NotNull(resultJwt!.Token);
    }
    
    [Fact]
    public async Task Test02_Post_Register_Returns_Status_BadRequest_When_Email_Already_Registered()
    {
        var registerData = IntTestsHelpers.RegisterData("Test1", "User1", "user@cinesta.ee", "Usercin1");
        
        var response = await _client.PostAsync(ApiUrl + "identity/account/register", registerData);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    // LOGIN METHOD
    [Fact]
    public async Task Test03_Post_Login_Returns_Status_OK_And_JWT_data()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        response.EnsureSuccessStatusCode();
        
        var requestContent = await response.Content.ReadAsStringAsync();
        var resultJwt = JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );
        Assert.NotNull(resultJwt!.Token);

    }
    
    [Fact]
    public async Task Test04_Get_Login_With_Wrong_Data_Returns_Status_NotFound()
    {
        var loginData = IntTestsHelpers.LoginData("owner@cinesta.ee", "Usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
    
    // REFRESHTOKEN METHOD
    [Fact]
    public async Task Test05_Post_RefreshToken_Returns_Status_OK_And_New_RefreshToken()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        response.EnsureSuccessStatusCode();
        
        var requestContent = await response.Content.ReadAsStringAsync();
        var resultJwt = JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );

        var data = IntTestsHelpers.RefreshTokenModel(resultJwt);
        
        response = await _client.PostAsync(ApiUrl + "identity/account/refreshToken", data);
        requestContent = await response.Content.ReadAsStringAsync();
        
        var resultRefreshToken = JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );
        
        Assert.NotEqual(resultJwt!.RefreshToken, resultRefreshToken!.RefreshToken );
    }
    
    // USERSLIST METHOD
    [Fact]
    public async Task Test06_Get_UsersList_Returns_List_Of_Users_For_Admin()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        
        var requestContent = await response.Content.ReadAsStringAsync();
        var resultJwt = JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );
        
        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Get;
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "identity/account/usersList");

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
        
        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var resultData = JsonSerializer.Deserialize<List<AppUser>>(apiContent,
        new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        Assert.NotNull(resultData);
        Assert.Equal(2, resultData!.Count);
    }
    
    [Fact]
    public async Task Test07_Get_UsersList_Returns_Forbidden_For_User()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);
        
        var requestContent = await response.Content.ReadAsStringAsync();
        var resultJwt = JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );
        
        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Get;
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt!.Token);
        apiRequest.RequestUri = new Uri(ApiUrl + "identity/account/usersList");

        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.Forbidden, apiResponse.StatusCode);
    }
    
    // CHANGEROLE METHOD
    [Fact]
    public async Task Test08_Post_ChangeRole_Returns_Forbidden_For_User()
    {
        var loginData = IntTestsHelpers.LoginData("user@cinesta.ee", "usercin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var resultJwt = await IntTestsHelpers.EntityFromResult<JwtResponse>(response);

        var roleUpdateForUser = new
        {
            Email = "admin@cinesta.ee",
            NewRole = "admin"
        };
        
        var jsonStr = JsonSerializer.Serialize(roleUpdateForUser);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var apiRequest = IntTestsHelpers.ApiRequest(HttpMethod.Post, resultJwt!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "identity/account/changerole");

        var apiResponse = await _client.SendAsync(apiRequest);
        Assert.Equal(HttpStatusCode.Forbidden, apiResponse.StatusCode);
    }

    [Fact]
    public async Task Test09_Post_ChangeRole_Returns_OK_For_Admin()
    {
        var loginData = IntTestsHelpers.LoginData("admin@cinesta.ee", "admincin");
        var response = await _client.PostAsync(ApiUrl + "identity/account/login", loginData);

        var requestContent = await response.Content.ReadAsStringAsync();
        var resultJwt = JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );

        var roleUpdateForUser = new
        {
            Email = "user@cinesta.ee",
            NewRole = "admin"
        };

        var jsonStr = JsonSerializer.Serialize(roleUpdateForUser);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Post;
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt!.Token);
        apiRequest.Content = data;
        apiRequest.RequestUri = new Uri(ApiUrl + "identity/account/changerole");

        var apiResponse = await _client.SendAsync(apiRequest);
        apiResponse.EnsureSuccessStatusCode();
    }
}