#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
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
public class ProfileFavoriteMoviesController : ControllerBase
{
    private readonly IAppPublic _public;

    public ProfileFavoriteMoviesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/ProfileFavoriteMovies
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
            new {id = profileFavoriteMovie.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()},
            res);
    }

    // DELETE: api/ProfileFavoriteMovies/5
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

    private async Task<bool> ProfileFavoriteMovieExists(Guid id)
    {
        return await _public.ProfileFavoriteMovie.ExistsAsync(id);
    }
}