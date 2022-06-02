#nullable disable
using App.Contracts.BLL;
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.MovieGenres;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  MovieGenre entities.
///     MovieGenre entities meant for between-connection of Movie entities with corresponding Genre entities.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MovieGenresController : ControllerBase
{
    private readonly IAppPublic _public;
    private readonly IAppBll _bll;

    /// <summary>
    ///     Constructor of MovieGenresController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public MovieGenresController(IAppPublic appPublic, IAppBll bll)
    {
        _public = appPublic;
        _bll = bll;
    }

    // GET: api/MovieGenres
    /// <summary>
    ///     Method returns list of all MovieGenre entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of generated from MovieGenre entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponseExample(200, typeof(GetListMovieGenresExample))]
    [ProducesResponseType(typeof(IEnumerable<MovieGenre>), 200)]
    [HttpGet]
    public async Task<IEnumerable<object>> GetMovieGenres(string culture)
    {
        return (await _bll.MovieGenre.IncludeGetAllAsync())
            .Select(m => new
            {
                Id = m.Id,
                MovieDetails = new
                {
                    Id = m.MovieDetailsId,
                    Title = m.MovieDetails!.Title.Translate(culture)
                },
                Genre = new
                {
                    Id = m.GenreId,
                    Naming = m.Genre!.Naming.Translate(culture)
                }
            });
    }

    // GET: api/MovieGenres/5
    /// <summary>
    ///     Method returns one exact MovieGenre entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieGenre entity Id</param>
    /// <returns>Generated from MovieGenre entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetMovieGenresExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetMovieGenre(Guid id, string culture)
    {
        var movieGenre = await _bll.MovieGenre.FirstOrDefaultAsync(id);

        if (movieGenre == null) return NotFound();

        return new
        {
            movieGenre.Id,
            MovieDetails = new
            {
                Id = movieGenre.MovieDetailsId,
                Title = movieGenre.MovieDetails!.Title.Translate(culture)
            },
            Genre = new
            {
                Id = movieGenre.GenreId,
                Naming = movieGenre.Genre!.Naming.Translate(culture)
            }
        };
    }

    // PUT: api/MovieGenres/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits MovieGenre entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieGenre entity id.</param>
    /// <param name="movieGenre">Updated MovieGenre entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [SwaggerRequestExample(typeof(MovieGenre), typeof(PostMovieGenresExample))]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieGenre(Guid id, MovieGenre movieGenre)
    {
        if (id != movieGenre.Id) return BadRequest();

        try
        {
            _public.MovieGenre.Update(movieGenre);
            await _public.SaveChangesAsync();
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
    /// <summary>
    ///     For admins and moderators only. Method adds new MovieGenre entity to API database
    /// </summary>
    /// <param name="movieGenre">MovieGenre class entity to add</param>
    /// <returns>Generated from MovieGenre entity object </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerResponseExample(201, typeof(PostMovieGenresExample))]
    [SwaggerRequestExample(typeof(MovieGenre), typeof(PostMovieGenresExample))]
    [HttpPost]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<object>> PostMovieGenre(MovieGenre movieGenre)
    {
        movieGenre.Id = Guid.NewGuid();
        _public.MovieGenre.Add(movieGenre);
        await _public.SaveChangesAsync();
        var res = new
        {
            movieGenre.Id,
            movieGenre.MovieDetailsId,
            movieGenre.GenreId
        };

        return CreatedAtAction("GetMovieGenre",
            new { id = movieGenre.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, res);
    }

    // DELETE: api/MovieGenres/5
    /// <summary>
    ///     For admins and moderators only. Deletes MovieGenre entity found by given id.
    /// </summary>
    /// <param name="id">MovieGenre entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieGenre(Guid id)
    {
        await _public.MovieGenre.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieGenreExists(Guid id)
    {
        return await _public.MovieGenre.ExistsAsync(id);
    }
}