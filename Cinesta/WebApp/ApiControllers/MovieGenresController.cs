#nullable disable
using App.Contracts.DAL;
using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class MovieGenresController : ControllerBase
{
    private readonly IAppUOW _uow;

    public MovieGenresController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/MovieGenres
    [HttpGet]
    public async Task<IEnumerable<MovieGenre>> GetMovieGenres()
    {
        return await _uow.MovieGenre.GetAllAsync();
    }

    // GET: api/MovieGenres/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieGenre>> GetMovieGenre(Guid id)
    {
        var movieGenre = await _uow.MovieGenre.FirstOrDefaultAsync(id);

        if (movieGenre == null) return NotFound();

        return movieGenre;
    }

    // PUT: api/MovieGenres/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovieGenre(Guid id, MovieGenre movieGenre)
    {
        if (id != movieGenre.Id) return BadRequest();

        try
        {
            _uow.MovieGenre.Update(movieGenre);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<MovieGenre>> PostMovieGenre(MovieGenre movieGenre)
    {
        _uow.MovieGenre.Add(movieGenre);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetMovieGenre", new {id = movieGenre.Id}, movieGenre);
    }

    // DELETE: api/MovieGenres/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieGenre(Guid id)
    {
        await _uow.MovieGenre.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieGenreExists(Guid id)
    {
        return await _uow.MovieGenre.ExistsAsync(id);
    }
}