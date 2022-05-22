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
    [HttpGet]
    public async Task<IEnumerable<ProfileMovie>> GetProfileMovies()
    {
        return await _public.ProfileMovie.GetAllAsync();
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

    // PUT: api/ProfileMovies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProfileMovie(Guid id, ProfileMovie profileMovie)
    {
        if (id != profileMovie.Id) return BadRequest();

        try
        {
            _public.ProfileMovie.Update(profileMovie);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProfileMovieExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/ProfileMovies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProfileMovie), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<ProfileMovie>> PostProfileMovie(ProfileMovie profileMovie)
    {
        _public.ProfileMovie.Add(profileMovie);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetProfileMovie",
            new {id = profileMovie.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, profileMovie);
    }

    // DELETE: api/ProfileMovies/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfileMovie(Guid id)
    {
        await _public.ProfileMovie.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProfileMovieExists(Guid id)
    {
        return await _public.ProfileMovie.ExistsAsync(id);
    }
}