#nullable disable
using App.Contracts.DAL;
using App.DAL.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class MovieDBScoresController : ControllerBase
{
    private readonly IAppUOW _uow;

    public MovieDBScoresController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/MovieDBScores
    [HttpGet]
    public async Task<IEnumerable<MovieDbScore>> GetMovieDbScores()
    {
        return await _uow.MovieDbScore.IncludeGetAllAsync();
    }

    // GET: api/MovieDBScores/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDbScore>> GetMovieDbScore(Guid id)
    {
        var movieDbScore = await _uow.MovieDbScore.IncludeFirstOrDefaultAsync(id);

        if (movieDbScore == null) return NotFound();

        return movieDbScore;
    }

    // PUT: api/MovieDBScores/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieDbScore(Guid id, MovieDbScore movieDbScore)
    {
        if (id != movieDbScore.Id) return BadRequest();

        try
        {
            _uow.MovieDbScore.Update(movieDbScore);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieDbScoreExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/MovieDBScores
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MovieDbScore>> PostMovieDbScore(MovieDbScore movieDbScore)
    {
        _uow.MovieDbScore.Add(movieDbScore);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetMovieDbScore", new {id = movieDbScore.Id}, movieDbScore);
    }

    // DELETE: api/MovieDBScores/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieDbScore(Guid id)
    {
        await _uow.MovieDbScore.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieDbScoreExists(Guid id)
    {
        return await _uow.MovieDbScore.ExistsAsync(id);
    }
}