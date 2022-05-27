#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  MovieType entities.
///     MovieType entities meant for storage of movie types namings (i.e. movie, carton, series).
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MovieTypesController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of MovieTypesController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public MovieTypesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/MovieTypes
    /// <summary>
    ///     Method returns list of all MovieType entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of MovieType entities.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<MovieType>), 200)]
    [HttpGet]
    public async Task<IEnumerable<MovieType>> GetMovieTypes()
    {
        return await _public.MovieType.GetAllAsync();
    }

    // GET: api/MovieTypes/5
    /// <summary>
    ///     Method returns one exact MovieType entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieType entity Id</param>
    /// <returns>MovieType class entity</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieType), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieType>> GetMovieType(Guid id)
    {
        var movieType = await _public.MovieType.FirstOrDefaultAsync(id);

        if (movieType == null) return NotFound();

        return movieType;
    }

    // PUT: api/MovieTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits MovieType entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieType entity id.</param>
    /// <param name="movieType">Updated MovieType entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieType(Guid id, MovieType movieType)
    {
        if (id != movieType.Id) return BadRequest();

        var movieTypeFromDb = await _public.MovieType.FirstOrDefaultAsync(id);
        if (movieTypeFromDb == null) return NotFound();

        try
        {
            movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
            _public.MovieType.Update(movieTypeFromDb);
            await _public.SaveChangesAsync();
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
    /// <summary>
    ///     For admins and moderators only. Method adds new MovieType entity to API database
    /// </summary>
    /// <param name="movieType">MovieType class entity to add</param>
    /// <returns>Added MovieType entity with it's id </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(MovieType), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MovieType>> PostMovieType(MovieType movieType)
    {
        movieType.Id = Guid.NewGuid();
        _public.MovieType.Add(movieType);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetMovieType",
            new { id = movieType.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, movieType);
    }

    // DELETE: api/MovieTypes/5
    /// <summary>
    ///     For admins and moderators only. Deletes MovieType entity found by given id.
    /// </summary>
    /// <param name="id">MovieType entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieType(Guid id)
    {
        _public.MovieType.Remove(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieTypeExists(Guid id)
    {
        return await _public.MovieType.ExistsAsync(id);
    }
}