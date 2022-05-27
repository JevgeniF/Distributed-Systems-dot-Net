#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.ProfileFavoriteMovies;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  ProfileFavoriteMovie entities.
///     ProfileFavoriteMovie entities meant for between-connection of user profile and moves, chosen as favorites.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileFavoriteMoviesController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of ProfileFavoriteMoviesController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public ProfileFavoriteMoviesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/ProfileFavoriteMovies
    /// <summary>
    ///     Method returns list of all ProfileFavoriteMovie entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of generated from ProfileFavoriteMovie entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [SwaggerResponseExample(200, typeof(GetProfileFavoriteMoviesExample))]
    [HttpGet("{profileId}")]
    public async Task<IEnumerable<object>> GetProfileFavoriteMoviesByProfileId(Guid profileId)
    {
        return (await _public.ProfileFavoriteMovie.IncludeGetAllByProfileIdAsync(profileId))
            .Select(p => new
            {
                p.Id,
                p.UserProfileId,
                MovieDetails = new
                {
                    p.MovieDetailsId,
                    p.MovieDetails!.PosterUri,
                    p.MovieDetails.Title
                }
            });
    }

    // POST: api/ProfileFavoriteMovies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     Method adds new ProfileFavoriteMovie entity to API database
    /// </summary>
    /// <param name="profileFavoriteMovie">ProfileFavoriteMovie class entity to add</param>
    /// <returns>Generated from ProfileFavoriteMovie entity object </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(ProfileFavoriteMovie), typeof(PostProfileFavoriteMoviesExample))]
    [SwaggerResponseExample(201, typeof(PostProfileFavoriteMoviesExample))]
    [HttpPost]
    public async Task<ActionResult<object>> PostProfileFavoriteMovie(
        ProfileFavoriteMovie profileFavoriteMovie)
    {
        profileFavoriteMovie.Id = Guid.NewGuid();
        _public.ProfileFavoriteMovie.Add(profileFavoriteMovie);
        await _public.SaveChangesAsync();

        var res = new
        {
            profileFavoriteMovie.Id,
            profileFavoriteMovie.UserProfileId,
            profileFavoriteMovie.MovieDetailsId
        };

        return CreatedAtAction("GetProfileFavoriteMoviesByProfileId",
            new { id = profileFavoriteMovie.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() },
            res);
    }

    // DELETE: api/ProfileFavoriteMovies/5
    /// <summary>
    ///     Deletes ProfileFavoriteMovie entity found by given id.
    /// </summary>
    /// <param name="id">ProfileFavoriteMovie entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfileFavoriteMovie(Guid id)
    {
        await _public.ProfileFavoriteMovie.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }
}