#nullable disable
using App.Contracts.DAL;
using App.Domain.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileFavoriteMoviesController : ControllerBase
{
    private readonly IAppUOW _uow;

    public ProfileFavoriteMoviesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/ProfileFavoriteMovies
    [HttpGet]
    public async Task<IEnumerable<ProfileFavoriteMovie>> GetProfileFavoriteMovies()
    {
        return await _uow.ProfileFavoriteMovie.GetAllAsync();
    }

    // GET: api/ProfileFavoriteMovies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileFavoriteMovie>> GetProfileFavoriteMovie(Guid id)
    {
        var profileFavoriteMovie = await _uow.ProfileFavoriteMovie.FirstOrDefaultAsync(id);

        if (profileFavoriteMovie == null) return NotFound();

        return profileFavoriteMovie;
    }

    // PUT: api/ProfileFavoriteMovies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProfileFavoriteMovie(Guid id, ProfileFavoriteMovie profileFavoriteMovie)
    {
        if (id != profileFavoriteMovie.Id) return BadRequest();

        try
        {
            _uow.ProfileFavoriteMovie.Update(profileFavoriteMovie);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<ProfileFavoriteMovie>> PostProfileFavoriteMovie(
        ProfileFavoriteMovie profileFavoriteMovie)
    {
        _uow.ProfileFavoriteMovie.Add(profileFavoriteMovie);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetProfileFavoriteMovie", new {id = profileFavoriteMovie.Id}, profileFavoriteMovie);
    }

    // DELETE: api/ProfileFavoriteMovies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfileFavoriteMovie(Guid id)
    {
        await _uow.ProfileFavoriteMovie.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProfileFavoriteMovieExists(Guid id)
    {
        return await _uow.ProfileFavoriteMovie.ExistsAsync(id);
    }
}