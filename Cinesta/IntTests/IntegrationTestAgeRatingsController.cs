using System.Net;
using System.Text;
using System.Text.Json;
using App.Public.DTO.v1.Identity;
using IntTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;


namespace IntegrationTests.ApiTests;

public class IntegrationTestAgeRatingsController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTestAgeRatingsController(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }
    
    [Fact]
    public async Task Get_AgeRatings_Returns_Unauthorized()
    {
        var response = await _client.GetAsync("/api/v1/AgeRatings");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_AgeRatings_Returns_OK()
    {
        var registerDto = new Register
        {
            Name = "Test",
            Surname = "User",
            Email = "user@cinesta.ee",
            Password = "User1.cin"
        };
        var jsonStr = JsonSerializer.Serialize(registerDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("https://cinesta.azurewebsites.net/api/v1/identity/Account/Register", data);
        
        var requestContent = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();


    }
}
