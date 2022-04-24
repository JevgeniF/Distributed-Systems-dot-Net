#nullable disable
using App.Contracts.DAL;
using App.Domain.MovieStandardDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class MovieTypesController : ControllerBase
{
    private readonly IAppUOW _uow;

    public MovieTypesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/MovieTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieTypeDto>>> GetMovieTypes()
    {
        var res = (await _uow.MovieType.GetAllAsync())
            .Select(m => new MovieTypeDto
            {
                Id = m.Id,
                Naming = m.Naming
            })
            .ToList();
        return res;
    }

    // GET: api/MovieTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieType>> GetMovieType(Guid id)
    {
        var movieType = await _uow.MovieType.FirstOrDefaultAsync(id);

        if (movieType == null) return NotFound();

        return movieType;
    }

    // PUT: api/MovieTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovieType(Guid id, MovieTypeDto movieType)
    {
        if (id != movieType.Id) return BadRequest();

        var movieTypeFromDb = await _uow.MovieType.FirstOrDefaultAsync(id);
        if (movieTypeFromDb == null) return NotFound();

        try
        {
            movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
            _uow.MovieType.Update(movieTypeFromDb);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<MovieType>> PostMovieType(MovieType movieType)
    {
        _uow.MovieType.Add(movieType);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetMovieType", new {id = movieType.Id}, movieType);
    }

    // DELETE: api/MovieTypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieType(Guid id)
    {
        _uow.MovieType.Remove(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieTypeExists(Guid id)
    {
        return await _uow.MovieType.ExistsAsync(id);
    }
}