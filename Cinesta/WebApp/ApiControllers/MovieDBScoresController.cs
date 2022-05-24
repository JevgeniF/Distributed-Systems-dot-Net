#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MovieDBScoresController : ControllerBase
{
    private readonly IAppPublic _public;

    public MovieDBScoresController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/MovieDBScores
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [SwaggerResponseExample(200, typeof(GetListMovieDBScoreExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetMovieDbScores()
    {
        return (await _public.MovieDbScore.IncludeGetAllAsync())
            .Select(m => new
            {
                m.Id,
                m.ImdbId,
                m.Score,
                m.MovieDetailsId
            });
    }

    // GET: api/MovieDBScores/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieDbScore), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetMovieDBScoreExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDbScore>> GetMovieDbScore(Guid id)
    {
        var movieDbScore = await _public.MovieDbScore.IncludeFirstOrDefaultAsync(id);

        if (movieDbScore == null) return NotFound();

        return movieDbScore;
    }

    // PUT: api/MovieDBScores/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(MovieDbScore), typeof(PostMovieDBScoreExample))]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieDbScore(Guid id, MovieDbScore movieDbScore)
    {
        if (id != movieDbScore.Id) return BadRequest();

        try
        {
            _public.MovieDbScore.Update(movieDbScore);
            await _public.SaveChangesAsync();
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
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(MovieDbScore), typeof(PostMovieDBScoreExample))]
    [SwaggerResponseExample(201, typeof(PostMovieDBScoreExample))]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<object>> PostMovieDbScore(MovieDbScore movieDbScore)
    {
        movieDbScore.Id = Guid.NewGuid();
        _public.MovieDbScore.Add(movieDbScore);
        await _public.SaveChangesAsync();

        var res = new
        {
            movieDbScore.Id,
            movieDbScore.ImdbId,
            movieDbScore.Score,
            movieDbScore.MovieDetailsId
        };

        return CreatedAtAction("GetMovieDbScore",
            new {id = movieDbScore.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, res);
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
        await _public.MovieDbScore.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieDbScoreExists(Guid id)
    {
        return await _public.MovieDbScore.ExistsAsync(id);
    }
}