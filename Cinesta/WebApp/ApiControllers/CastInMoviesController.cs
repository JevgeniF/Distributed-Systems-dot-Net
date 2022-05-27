#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.CastInMovie;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for getting, adding, editing or deletion of cast.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CastInMoviesController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Cast in movies controller's constructor.
    /// </summary>
    /// <param name="appPublic">Takes in public layer interface</param>
    public CastInMoviesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/CastInMovies
    /// <summary>
    ///     Get cast (actors, directors, etc) for all movies in database.
    /// </summary>
    /// <returns>List of cast for movies</returns>
    /// <example>[{"id": "95f44037-90dd-4224-bf80-c65f44a01c9b"}]</example>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [SwaggerResponseExample(200, typeof(GetListCastInMovieExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetCastInMovies()
    {
        return (await _public.CastInMovie.IncludeGetAllAsync())
            .Select(c => new
            {
                c.Id,
                CastRole = new CastRole
                {
                    Id = c.CastRoleId,
                    Naming = c.CastRole!.Naming
                },
                Person = new Person
                {
                    Id = c.PersonId,
                    Name = c.Persons!.Name,
                    Surname = c.Persons.Surname
                },
                MovieDetails = new
                {
                    Id = c.MovieDetailsId, c.MovieDetails!.Title
                }
            });
    }

    // GET: api/CastInMovies/5
    /// <summary>
    ///     Returns one specific cast from API database by id
    /// </summary>
    /// <param name="id">Id of queryable cast</param>
    /// <returns>Cast on movie entity</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetCastInMovieExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetCastInMovie(Guid id)
    {
        var castInMovie = await _public.CastInMovie.IncludeFirstOrDefaultAsync(id);
        if (castInMovie == null) return NotFound();

        return new
        {
            castInMovie.Id,
            CastRole = new CastRole
            {
                Id = castInMovie.CastRoleId,
                Naming = castInMovie.CastRole!.Naming
            },
            Person = new Person
            {
                Id = castInMovie.PersonId,
                Name = castInMovie.Persons!.Name,
                Surname = castInMovie.Persons.Surname
            },
            MovieDetails = new
            {
                Id = castInMovie.MovieDetailsId,
                castInMovie.MovieDetails!.Title
            }
        };
    }

    // PUT: api/CastInMovies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     Edit one specific cast in API database, queryable by id.
    /// </summary>
    /// <param name="id">Id of cast to edit</param>
    /// <param name="castInMovie">Updated cast data</param>
    /// <returns>Nothing</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(CastInMovie), typeof(PostCastInMovieExample))]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutCastInMovie(Guid id, CastInMovie castInMovie)
    {
        if (id != castInMovie.Id) return BadRequest();

        try
        {
            _public.CastInMovie.Update(castInMovie);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CastInMovieExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/CastInMovies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     Add new cast to API database.
    /// </summary>
    /// <param name="castInMovie">New cast in movie entity</param>
    /// <returns>Nothing</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(CastInMovie), typeof(PostCastInMovieExample))]
    [SwaggerResponseExample(200, typeof(PostCastInMovieExample))]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<object>> PostCastInMovie(CastInMovie castInMovie)
    {
        castInMovie.Id = Guid.NewGuid();
        _public.CastInMovie.Add(castInMovie);
        await _public.SaveChangesAsync();

        var res = new
        {
            castInMovie.Id,
            castInMovie.CastRoleId,
            castInMovie.PersonId,
            castInMovie.MovieDetailsId
        };

        return CreatedAtAction("GetCastInMovie",
            new { id = castInMovie.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, res);
    }

    // DELETE: api/CastInMovies/5
    /// <summary>
    ///     Delete one specific cast in movie entity from API database.
    /// </summary>
    /// <param name="id">Id of cast in movie entity for deletion</param>
    /// <returns>Nothing</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteCastInMovie(Guid id)
    {
        await _public.CastInMovie.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CastInMovieExists(Guid id)
    {
        return await _public.CastInMovie.ExistsAsync(id);
    }
}