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
public class MovieDBScoresController : ControllerBase
{
    private readonly IAppBll _bll;

    public MovieDBScoresController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/MovieDBScores
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<MovieDbScore>), 200)]
    [HttpGet]
    public async Task<IEnumerable<MovieDbScore>> GetMovieDbScores()
    {
        return await _bll.MovieDbScore.IncludeGetAllAsync();
    }

    // GET: api/MovieDBScores/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieDbScore), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDbScore>> GetMovieDbScore(Guid id)
    {
        var movieDbScore = await _bll.MovieDbScore.IncludeFirstOrDefaultAsync(id);

        if (movieDbScore == null) return NotFound();

        return movieDbScore;
    }

    // PUT: api/MovieDBScores/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieDbScore(Guid id, MovieDbScore movieDbScore)
    {
        if (id != movieDbScore.Id) return BadRequest();

        try
        {
            _bll.MovieDbScore.Update(movieDbScore);
            await _bll.SaveChangesAsync();
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieDbScore),201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MovieDbScore>> PostMovieDbScore(MovieDbScore movieDbScore)
    {
        _bll.MovieDbScore.Add(movieDbScore);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetMovieDbScore", new {id = movieDbScore.Id,  version = HttpContext.GetRequestedApiVersion()!.ToString()}, movieDbScore);
    }

    // DELETE: api/MovieDBScores/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieDbScore(Guid id)
    {
        await _bll.MovieDbScore.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieDbScoreExists(Guid id)
    {
        return await _bll.MovieDbScore.ExistsAsync(id);
    }
}