#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MovieGenresController : ControllerBase
{
    private readonly IAppBll _bll;

    public MovieGenresController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/MovieGenres
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<MovieGenre>), 200)]
    [HttpGet]
    public async Task<IEnumerable<MovieGenre>> GetMovieGenres()
    {
        return await _bll.MovieGenre.GetAllAsync();
    }

    // GET: api/MovieGenres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieGenre), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieGenre>> GetMovieGenre(Guid id)
    {
        var movieGenre = await _bll.MovieGenre.FirstOrDefaultAsync(id);

        if (movieGenre == null) return NotFound();

        return movieGenre;
    }

    // PUT: api/MovieGenres/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieGenre(Guid id, MovieGenre movieGenre)
    {
        if (id != movieGenre.Id) return BadRequest();

        try
        {
            _bll.MovieGenre.Update(movieGenre);
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieGenreExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/MovieGenres
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieGenre),201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MovieGenre>> PostMovieGenre(MovieGenre movieGenre)
    {
        _bll.MovieGenre.Add(movieGenre);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetMovieGenre", new {id = movieGenre.Id,  version = HttpContext.GetRequestedApiVersion()!.ToString()}, movieGenre);
    }

    // DELETE: api/MovieGenres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieGenre(Guid id)
    {
        await _bll.MovieGenre.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieGenreExists(Guid id)
    {
        return await _bll.MovieGenre.ExistsAsync(id);
    }
}