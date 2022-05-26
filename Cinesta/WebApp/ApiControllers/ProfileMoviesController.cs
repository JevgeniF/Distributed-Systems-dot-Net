#nullable disable
using App.Contracts.Public;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileMoviesController : ControllerBase
{
    private readonly IAppPublic _public;

    public ProfileMoviesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/ProfileMovies
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [SwaggerResponseExample(200, typeof(GetProfileMoviesExample))]
    [HttpGet("{profileId}")]
    public async Task<IEnumerable<object>> GetAllowedProfileMovies(Guid profileId)
    {
        var profile = await _public.UserProfile.FirstOrDefaultAsync(profileId);

        if (profile == null) return Array.Empty<object>();
        return (await _public.MovieDetails.IncludeGetByAgeAsync(profile.Age))
            .Select(m => new
            {
                m.Id,
                UserProfileId = profile.Id,
                MovieDetails = new
                {
                    m.Id,
                    m.PosterUri,
                    m.Title
                }
            });
    }
}