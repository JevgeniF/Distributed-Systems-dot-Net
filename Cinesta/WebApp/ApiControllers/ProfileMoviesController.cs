#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ProfileMoviesController : ControllerBase
{
    private readonly IAppUOW _uow;

    public ProfileMoviesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/ProfileMovies
    [HttpGet]
    public async Task<IEnumerable<ProfileMovie>> GetProfileMovies()
    {
        return await _uow.ProfileMovie.GetAllAsync();
    }

    // GET: api/ProfileMovies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileMovie>> GetProfileMovie(Guid id)
    {
        var profileMovie = await _uow.ProfileMovie.FirstOrDefaultAsync(id);

        if (profileMovie == null) return NotFound();

        return profileMovie;
    }

    // PUT: api/ProfileMovies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProfileMovie(Guid id, ProfileMovie profileMovie)
    {
        if (id != profileMovie.Id) return BadRequest();

        try
        {
            _uow.ProfileMovie.Update(profileMovie);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<ProfileMovie>> PostProfileMovie(ProfileMovie profileMovie)
    {
        _uow.ProfileMovie.Add(profileMovie);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetProfileMovie", new {id = profileMovie.Id}, profileMovie);
    }

    // DELETE: api/ProfileMovies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfileMovie(Guid id)
    {
        await _uow.ProfileMovie.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProfileMovieExists(Guid id)
    {
        return await _uow.ProfileMovie.ExistsAsync(id);
    }
}