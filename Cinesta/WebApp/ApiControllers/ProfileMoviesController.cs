#nullable disable
using System.Diagnostics;
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [ProducesResponseType(typeof(IEnumerable<ProfileMovie>), 200)]
    [HttpGet("{profileId}")]
    public async Task<IEnumerable<MovieDetails>> GetAllowedProfileMovies(Guid profileId)
    {
        var profile = await _public.UserProfile.FirstOrDefaultAsync(profileId);

        if (profile != null) return await _public.MovieDetails.IncludeGetByAgeAsync(profile.Age);
        return null;
        //TODO FIX THIS THING
    }

    // GET: api/ProfileMovies/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProfileMovie), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileMovie>> GetProfileMovie(Guid id)
    {
        var profileMovie = await _public.ProfileMovie.FirstOrDefaultAsync(id);

        if (profileMovie == null) return NotFound();

        return profileMovie;
    }
}