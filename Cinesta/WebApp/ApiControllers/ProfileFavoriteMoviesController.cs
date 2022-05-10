#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ProfileFavoriteMoviesController : ControllerBase
{
    private readonly IAppBll _bll;

    public ProfileFavoriteMoviesController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/ProfileFavoriteMovies
    [HttpGet]
    public async Task<IEnumerable<ProfileFavoriteMovie>> GetProfileFavoriteMovies()
    {
        return await _bll.ProfileFavoriteMovie.GetAllAsync();
    }

    // GET: api/ProfileFavoriteMovies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileFavoriteMovie>> GetProfileFavoriteMovie(Guid id)
    {
        var profileFavoriteMovie = await _bll.ProfileFavoriteMovie.FirstOrDefaultAsync(id);

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
            _bll.ProfileFavoriteMovie.Update(profileFavoriteMovie);
            await _bll.SaveChangesAsync();
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
        _bll.ProfileFavoriteMovie.Add(profileFavoriteMovie);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetProfileFavoriteMovie", new {id = profileFavoriteMovie.Id}, profileFavoriteMovie);
    }

    // DELETE: api/ProfileFavoriteMovies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfileFavoriteMovie(Guid id)
    {
        await _bll.ProfileFavoriteMovie.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProfileFavoriteMovieExists(Guid id)
    {
        return await _bll.ProfileFavoriteMovie.ExistsAsync(id);
    }
}