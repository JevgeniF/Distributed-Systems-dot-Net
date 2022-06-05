using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;

namespace IntTests;

public class IntTestsHelpers
{
    public static HttpRequestMessage ApiRequest(HttpMethod method, string token)
    {
        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = method;
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return apiRequest;
    }

    public static async Task<TEntity?> EntityFromResult<TEntity>(HttpResponseMessage response)
    {
        var requestContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TEntity>(requestContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        return result;
    }

    public static StringContent RefreshTokenModel(JwtResponse? resultJwt)
    {
        var refreshTokenModel = new RefreshTokenModel
        {
            Jwt = resultJwt!.Token,
            RefreshToken = resultJwt.RefreshToken
        };
        var jsonStr = JsonSerializer.Serialize(refreshTokenModel);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }

    public static StringContent RegisterData(string name, string surname, string email, string password)
    {
        var registerDto = new Register
        {
            Name = name,
            Surname = surname,
            Email = email,
            Password = password
        };
        
        var jsonStr = JsonSerializer.Serialize(registerDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }

    public static StringContent LoginData(string email, string password)
    {
        var loginDto = new Login
        {
            Email = email,
            Password = password
        };
        
        var jsonStr = JsonSerializer.Serialize(loginDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }
    
    public static StringContent AgeRatingData(Guid? id, string naming, int allowedAge)
    {
        var ageRating = new AgeRating
        {
            Naming = naming,
            AllowedAge = allowedAge
        };

        if (id != null)
        {
            ageRating.Id = (Guid) id;
        }
        
        var jsonStr = JsonSerializer.Serialize(ageRating);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }
    
    public static StringContent CastRoleData(Guid? id, string naming)
    {
        var castRole = new CastRole
        {
            Naming = naming
        };

        if (id != null)
        {
            castRole.Id = (Guid) id;
        }
        
        var jsonStr = JsonSerializer.Serialize(castRole);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }

    public static TEntity ResultData<TEntity>(string apiContent)
    {
        var resultData = System.Text.Json.JsonSerializer.Deserialize<TEntity>(apiContent,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        return resultData!;
    }
}