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
public class MovieTypesController : ControllerBase
{
    private readonly IAppPublic _public;

    public MovieTypesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/MovieTypes
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<MovieType>), 200)]
    [HttpGet]
    public async Task<IEnumerable<MovieType>> GetMovieTypes()
    {
        return await _public.MovieType.GetAllAsync();
    }

    // GET: api/MovieTypes/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieType), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieType>> GetMovieType(Guid id)
    {
        var movieType = await _public.MovieType.FirstOrDefaultAsync(id);

        if (movieType == null) return NotFound();

        return movieType;
    }

    // PUT: api/MovieTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieType(Guid id, MovieType movieType)
    {
        if (id != movieType.Id) return BadRequest();

        var movieTypeFromDb = await _public.MovieType.FirstOrDefaultAsync(id);
        if (movieTypeFromDb == null) return NotFound();

        try
        {
            movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
            _public.MovieType.Update(movieTypeFromDb);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieTypeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/MovieTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieType), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MovieType>> PostMovieType(MovieType movieType)
    {
        movieType.Id = Guid.NewGuid();
        _public.MovieType.Add(movieType);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetMovieType",
            new {id = movieType.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, movieType);
    }

    // DELETE: api/MovieTypes/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieType(Guid id)
    {
        _public.MovieType.Remove(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieTypeExists(Guid id)
    {
        return await _public.MovieType.ExistsAsync(id);
    }
}