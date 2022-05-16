#nullable disable
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;
using Genre = App.Public.DTO.v1.Genre;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GenresController : ControllerBase
{
    private readonly IAppBll _bll;

    public GenresController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/Genres
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.BLL.DTO.Genre>), 200)]
    [HttpGet]
    public async Task<IEnumerable<App.BLL.DTO.Genre>> GetGenres()
    {
        return await _bll.Genre.GetAllAsync();
    }

    // GET: api/Genres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.BLL.DTO.Genre), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.BLL.DTO.Genre>> GetGenre(Guid id)
    {
        var genre = await _bll.Genre.FirstOrDefaultAsync(id);

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

        var genreFromDb = await _bll.Genre.FirstOrDefaultAsync(id);
        if (genreFromDb == null) return NotFound();

        try
        {
            genreFromDb.Naming.SetTranslation(genre.Naming);
            _bll.Genre.Update(genreFromDb);
            await _bll.SaveChangesAsync();
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
    [ProducesResponseType(typeof(App.BLL.DTO.Genre),201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<App.BLL.DTO.Genre>> PostGenre(App.BLL.DTO.Genre genre)
    {
        _bll.Genre.Add(genre);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetGenre", new {id = genre.Id,  version = HttpContext.GetRequestedApiVersion()!.ToString()}, genre);
    }

    // DELETE: api/Genres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        await _bll.Genre.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> GenreExists(Guid id)
    {
        return await _bll.Genre.ExistsAsync(id);
    }
}