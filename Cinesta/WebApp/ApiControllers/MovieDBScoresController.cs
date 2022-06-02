#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.MovieDbScore;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  MovieDBScore entities.
///     MovieDBScore entities meant for storage of movie id and score from IMDB for future communication with IMDB Api.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MovieDBScoresController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of MovieDBScoresController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public MovieDBScoresController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/MovieDBScores
    /// <summary>
    ///     Method returns list of all MovieDBScore entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of generated from MovieDBScore entity object</returns>
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
    
    // GET: api/MovieDBScores
    /// <summary>
    ///     Method returns MovieDBScore entity stored in API database.
    /// </summary>
    /// <returns>IEnumerable of generated from MovieDBScore entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [SwaggerResponseExample(200, typeof(GetListMovieDBScoreExample))]
    [HttpGet ("movie={movieId}")]
    public async Task<object> GetMovieDbScoresForMovie(Guid movieId)
    {
        var movieDbScore = await _public.MovieDbScore.GetMovieDbScoresForMovie(movieId);
        if (movieDbScore == null) return NotFound();

        return new
        {
            movieDbScore.Id,
            movieDbScore.ImdbId,
            movieDbScore.Score,
            movieDbScore.MovieDetailsId
        };
    }

    // GET: api/MovieDBScores/5
    /// <summary>
    ///     Method returns one exact MovieDBScore entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieDBScore entity Id</param>
    /// <returns>Generated from MovieDBScore entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetMovieDBScoreExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetMovieDbScore(Guid id)
    {
        var movieDbScore = await _public.MovieDbScore.IncludeFirstOrDefaultAsync(id);

        if (movieDbScore == null) return NotFound();

        return new
        {
            movieDbScore.Id,
            movieDbScore.ImdbId,
            movieDbScore.Score,
            movieDbScore.MovieDetailsId
        };
    }

    // PUT: api/MovieDBScores/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits MovieDBScore entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieDBScore entity id.</param>
    /// <param name="movieDbScore">Updated MovieDBScore entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(MovieDbScore), typeof(PostMovieDBScoreExample))]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    /// <summary>
    ///     For admins and moderators only. Method adds new MovieDBScore entity to API database
    /// </summary>
    /// <param name="movieDbScore">MovieDBScore class entity to add</param>
    /// <returns>Generated from MovieDbScore entity object </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(MovieDbScore), typeof(PostMovieDBScoreExample))]
    [SwaggerResponseExample(201, typeof(PostMovieDBScoreExample))]
    [HttpPost]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            new { id = movieDbScore.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, res);
    }

    // DELETE: api/MovieDBScores/5
    /// <summary>
    ///     For admins and moderators only. Deletes MovieDBScore entity found by given id.
    /// </summary>
    /// <param name="id">MovieDBScore entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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