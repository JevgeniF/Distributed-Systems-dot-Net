using System.Net.Http.Headers;
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
    public static StringContent GenreData(Guid? id, string naming)
    {
        var genre = new CastRole
        {
            Naming = naming
        };

        if (id != null)
        {
            genre.Id = (Guid) id;
        }
        
        var jsonStr = JsonSerializer.Serialize(genre);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }
    public static StringContent MovieTypeData(Guid? id, string naming)
    {
        var movieType = new MovieType
        {
            Naming = naming
        };

        if (id != null)
        {
            movieType.Id = (Guid) id;
        }
        
        var jsonStr = JsonSerializer.Serialize(movieType);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }
    public static StringContent PersonData(Guid? id, string name, string surname)
    {
        var person = new Person
        {
            Name = name,
            Surname = surname
        };

        if (id != null)
        {
            person.Id = (Guid) id;
        }
        
        var jsonStr = JsonSerializer.Serialize(person);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }
    public static StringContent SubscriptionData(Guid? id, string naming, string description, int profilesCount, double price)
    {
        var subscription = new Subscription()
        {
            Naming = naming,
            Description = description,
            ProfilesCount = 0,
            Price = 0
        };

        if (id != null)
        {
            subscription.Id = (Guid) id;
        }
        
        var jsonStr = JsonSerializer.Serialize(subscription);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }
    
    public static StringContent MovieDetailsData(Guid? id, string posterUri, string title, DateTime releaseDate,
        string description, Guid ageRatingId, AgeRating? ageRating, Guid movieTypeId, MovieType? movieType)
    {
        var movieDetails = new MovieDetails
        {
            PosterUri = posterUri,
            Title = title,
            Released = releaseDate,
            Description= description,
            AgeRatingId = ageRatingId,
            MovieTypeId = movieTypeId,
        };

        if (id != null)
        {
            movieDetails.Id = (Guid) id;
        }

        if (ageRating != null)
        {
            movieDetails.AgeRating = ageRating;
        }

        if (movieType != null)
        {
            movieDetails.MovieType = movieType;
        }
        
        var jsonStr = JsonSerializer.Serialize(movieDetails);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        return data;
    }
    
    public static StringContent VideoData(Guid? id, int season, string title, string fileUri,
        string description, Guid movieDetailsId, MovieDetails? movieDetails)
    {
        var video = new Video
        {
            Season = season,
            Title = title,
            FileUri = fileUri,
            Duration = DateTime.Now,
            Description= description,
            MovieDetailsId = movieDetailsId
        };

        if (id != null)
        {
            video.Id = (Guid) id;
        }

        if (movieDetails != null)
        {
            video.MovieDetails = movieDetails;
        }
        
        var jsonStr = JsonSerializer.Serialize(video);
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