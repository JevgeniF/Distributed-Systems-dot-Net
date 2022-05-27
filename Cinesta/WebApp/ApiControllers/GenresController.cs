#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  Genre entities.
///     Genre entities meant for storage of movies genres namings.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GenresController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of GenresController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public GenresController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/Genres
    /// <summary>
    ///     Method returns list of all Genre entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of Genre entities.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Genre>), 200)]
    [Authorize(Roles = "admin,moderator,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public async Task<IEnumerable<Genre>> GetGenres()
    {
        return await _public.Genre.GetAllAsync();
    }

    // GET: api/Genres/5
    /// <summary>
    ///     Method returns one exact Genre entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: Genre entity Id</param>
    /// <returns>Genre class entity</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Genre), 200)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "admin,moderator,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(Guid id)
    {
        var genre = await _public.Genre.FirstOrDefaultAsync(id);

        if (genre == null) return NotFound();

        return genre;
    }

    // PUT: api/Genres/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits Genre entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: Genre entity id.</param>
    /// <param name="genre">Updated Genre entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGenre(Guid id, Genre genre)
    {
        if (id != genre.Id) return BadRequest();

        var genreFromDb = await _public.Genre.FirstOrDefaultAsync(id);
        if (genreFromDb == null) return NotFound();

        try
        {
            genreFromDb.Naming.SetTranslation(genre.Naming);
            _public.Genre.Update(genreFromDb);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GenreExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Genres
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method adds new Genre entity to API database
    /// </summary>
    /// <param name="genre">Genre class entity to add</param>
    /// <returns>Added Genre entity with it's id </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Genre), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<Genre>> PostGenre(Genre genre)
    {
        genre.Id = Guid.NewGuid();
        _public.Genre.Add(genre);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetGenre",
            new { id = genre.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, genre);
    }

    // DELETE: api/Genres/5
    /// <summary>
    ///     For admins and moderators only. Deletes Genre entity found by given id.
    /// </summary>
    /// <param name="id">Genre entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        await _public.Genre.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> GenreExists(Guid id)
    {
        return await _public.Genre.ExistsAsync(id);
    }
}