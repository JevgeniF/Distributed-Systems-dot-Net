#nullable disable
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
    [ProducesResponseType(typeof(IEnumerable<ProfileFavoriteMovie>), 200)]
    [HttpGet]
    public async Task<IEnumerable<ProfileFavoriteMovie>> GetProfileFavoriteMovies()
    {
        return await _public.ProfileFavoriteMovie.GetAllAsync();
    }

    // GET: api/ProfileFavoriteMovies/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProfileFavoriteMovie), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileFavoriteMovie>> GetProfileFavoriteMovie(Guid id)
    {
        var profileFavoriteMovie = await _public.ProfileFavoriteMovie.FirstOrDefaultAsync(id);

        if (profileFavoriteMovie == null) return NotFound();

        return profileFavoriteMovie;
    }

    // PUT: api/ProfileFavoriteMovies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProfileFavoriteMovie(Guid id, ProfileFavoriteMovie profileFavoriteMovie)
    {
        if (id != profileFavoriteMovie.Id) return BadRequest();

        try
        {
            _public.ProfileFavoriteMovie.Update(profileFavoriteMovie);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProfileFavoriteMovieExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/ProfileFavoriteMovies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProfileFavoriteMovie), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<ProfileFavoriteMovie>> PostProfileFavoriteMovie(
        ProfileFavoriteMovie profileFavoriteMovie)
    {
        _public.ProfileFavoriteMovie.Add(profileFavoriteMovie);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetProfileFavoriteMovie",
            new {id = profileFavoriteMovie.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()},
            profileFavoriteMovie);
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