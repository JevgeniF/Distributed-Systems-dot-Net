#nullable disable
using App.Contracts.Public;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.ProfileMovies;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  ProfileMovie entities.
///     ProfileMovie unnecessary entity used to make listing of profile movies more easy.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileMoviesController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of ProfileMoviesController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public ProfileMoviesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/ProfileMovies/profileId
    /// <summary>
    ///     Method returns list of all MovieDetails entities stored in API database and allowed by profile age.
    /// </summary>
    /// <returns>IEnumerable of generated from ProfileMovie entity object</returns>
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