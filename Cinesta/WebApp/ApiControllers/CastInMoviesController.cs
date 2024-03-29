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
using WebApp.SwaggerExamples.CastInMovie;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for getting, adding, editing or deletion of cast.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CastInMoviesController : ControllerBase
{
    private readonly IAppPublic _public;
    private readonly IAppBll _bll;

    /// <summary>
    ///     Cast in movies controller's constructor.
    /// </summary>
    /// <param name="appPublic">Takes in public layer interface</param>
    public CastInMoviesController(IAppPublic appPublic, IAppBll bll)
    {
        _public = appPublic;
        _bll = bll;
    }

    // GET: api/CastInMovies
    /// <summary>
    ///     Get cast (actors, directors, etc) for all movies in database.
    /// </summary>
    /// <returns>List of cast for movies</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [SwaggerResponseExample(200, typeof(GetListCastInMovieExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetCast(string culture)
    {
        return (await _bll.CastInMovie.IncludeGetAllAsync())
            .Select(c => new
            {
                c.Id,
                CastRole = new CastRole
                {
                    Id = c.CastRoleId,
                    Naming = c.CastRole!.Naming.Translate(culture)!
                },
                Person = new Person
                {
                    Id = c.PersonId,
                    Name = c.Persons!.Name,
                    Surname = c.Persons.Surname
                },
                MovieDetails = new
                {
                    Id = c.MovieDetailsId,
                    Title = c.MovieDetails!.Title.Translate(culture)
                }
            });
    }
    
    // GET: api/CastInMovies/movie/5
    /// <summary>
    ///     Get cast (actors, directors, etc) for all movies in database.
    /// </summary>
    /// <returns>List of cast for movies</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [SwaggerResponseExample(200, typeof(GetCastInMovieExample))]
    [HttpGet ("movie={movieId}")]
    public async Task<IEnumerable<object>> GetCastInMovie(Guid movieId , string culture)
    {
        return (await _bll.CastInMovie.GetByMovie(movieId))
            .Select(c => new
            {
                c.Id,
                CastRole = new CastRole
                {
                    Id = c.CastRoleId,
                    Naming = c.CastRole!.Naming.Translate(culture)!
                },
                Person = new Person
                {
                    Id = c.PersonId,
                    Name = c.Persons!.Name,
                    Surname = c.Persons.Surname
                },
                MovieDetails = new
                {
                    Id = c.MovieDetailsId,
                    Title = c.MovieDetails!.Title.Translate(culture)
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
    public async Task<ActionResult<object>> GetCast(Guid id, string culture)
    {
        var castInMovie = await _bll.CastInMovie.IncludeFirstOrDefaultAsync(id);
        if (castInMovie == null) return NotFound();

        return new
        {
            castInMovie.Id,
            CastRole = new CastRole
            {
                Id = castInMovie.CastRoleId,
                Naming = castInMovie.CastRole!.Naming.Translate(culture)!
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
                Title = castInMovie.MovieDetails!.Title.Translate(culture)
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
    public async Task<IActionResult> PutCast(Guid id, CastInMovie castInMovie, string culture)
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
    public async Task<ActionResult<object>> PostCast(CastInMovie castInMovie, string culture)
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
    public async Task<IActionResult> DeleteCast(Guid id)
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