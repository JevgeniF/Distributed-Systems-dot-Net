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
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GenresController : ControllerBase
{
    private readonly IAppPublic _public;

    public GenresController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/Genres
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Genre>), 200)]
    [HttpGet]
    public async Task<IEnumerable<Genre>> GetGenres()
    {
        return await _public.Genre.GetAllAsync();
    }

    // GET: api/Genres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Genre), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(Guid id)
    {
        var genre = await _public.Genre.FirstOrDefaultAsync(id);

        if (genre == null) return NotFound();

        return genre;
    }

    // PUT: api/Genres/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGenre(Guid id, Genre genre)
    {
        if (id != genre.Id) return BadRequest();

        var genreFromDb = await _public.Genre.FirstOrDefaultAsync(id);
        if (genreFromDb == null) return NotFound();

        try
        {
            genreFromDb.Naming.SetTranslation(genre.Naming);
            _public.Genre.Update(genreFromDb);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GenreExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Genres
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Genre), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<Genre>> PostGenre(Genre genre)
    {
        genre.Id = Guid.NewGuid();
        _public.Genre.Add(genre);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetGenre",
            new {id = genre.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, genre);
    }

    // DELETE: api/Genres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        await _public.Genre.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> GenreExists(Guid id)
    {
        return await _public.Genre.ExistsAsync(id);
    }
}