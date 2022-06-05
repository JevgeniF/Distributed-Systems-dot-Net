using Microsoft.AspNetCore.Mvc.Testing;

namespace IntTests;

public class IntegrationTestHomeController: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public IntegrationTestHomeController(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }

    [Fact]
    public async Task Get_Index()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
    
}