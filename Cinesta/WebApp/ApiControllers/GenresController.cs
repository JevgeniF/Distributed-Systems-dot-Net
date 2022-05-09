#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IAppUOW _uow;

    public GenresController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/Genres
    [HttpGet]
    public async Task<IEnumerable<Genre>> GetGenres()
    {
        return await _uow.Genre.GetAllAsync();
    }

    // GET: api/Genres/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(Guid id)
    {
        var genre = await _uow.Genre.FirstOrDefaultAsync(id);

        if (genre == null) return NotFound();

        return genre;
    }

    // PUT: api/Genres/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGenre(Guid id, GenreDto genre)
    {
        if (id != genre.Id) return BadRequest();

        var genreFromDb = await _uow.Genre.FirstOrDefaultAsync(id);
        if (genreFromDb == null) return NotFound();

        try
        {
            genreFromDb.Naming.SetTranslation(genre.Naming);
            _uow.Genre.Update(genreFromDb);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<Genre>> PostGenre(Genre genre)
    {
        _uow.Genre.Add(genre);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetGenre", new {id = genre.Id}, genre);
    }

    // DELETE: api/Genres/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        await _uow.Genre.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> GenreExists(Guid id)
    {
        return await _uow.Genre.ExistsAsync(id);
    }
}