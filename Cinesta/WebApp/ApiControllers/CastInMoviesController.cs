#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CastInMoviesController : ControllerBase
{
    private readonly IAppPublic _public;

    public CastInMoviesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/CastInMovies
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<CastInMovie>), 200)]
    [HttpGet]
    public async Task<IEnumerable<CastInMovie>> GetCastInMovies()
    {
        return (await _public.CastInMovie.IncludeGetAllAsync()).Select(c => new CastInMovie
        {
            Id = c.Id,
            CastRoleId = c.CastRoleId,
            CastRole = new CastRole
            {
                Id = c.CastRole!.Id,
                Naming = c.CastRole.Naming
            },
            PersonId = c.PersonId,
            Persons = c.Persons,
            MovieDetailsId = c.MovieDetailsId,
            MovieDetails = c.MovieDetails
        });
    }

    // GET: api/CastInMovies/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CastInMovie), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CastInMovie>> GetCastInMovie(Guid id)
    {
        var castInMovie = await _public.CastInMovie.IncludeFirstOrDefaultAsync(id);
        if (castInMovie == null) return NotFound();
        var castInMovieDto = new CastInMovie
        {
            Id = castInMovie.Id,
            CastRoleId = castInMovie.CastRoleId,
            CastRole = new CastRole
            {
                Id = castInMovie.CastRole!.Id,
                Naming = castInMovie.CastRole.Naming
            },
            PersonId = castInMovie.PersonId,
            Persons = castInMovie.Persons,
            MovieDetailsId = castInMovie.MovieDetailsId,
            MovieDetails = castInMovie.MovieDetails
        };

        return castInMovieDto;
    }

    // PUT: api/CastInMovies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutCastInMovie(Guid id, CastInMovie castInMovie)
    {
        if (id != castInMovie.Id) return BadRequest();

        try
        {
            _public.CastInMovie.Update(new CastInMovie
            {
                Id = castInMovie.Id,
                CastRoleId = castInMovie.CastRoleId,
                CastRole = new CastRole
                {
                    Id = castInMovie.CastRole!.Id,
                    Naming = castInMovie.CastRole.Naming
                },
                PersonId = castInMovie.PersonId,
                Persons = castInMovie.Persons,
                MovieDetailsId = castInMovie.MovieDetailsId,
                MovieDetails = castInMovie.MovieDetails
            });
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CastInMovie), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<CastInMovie>> PostCastInMovie(CastInMovie castInMovie)
    {
        _public.CastInMovie.Add(new CastInMovie
        {
            Id = castInMovie.Id,
            CastRoleId = castInMovie.CastRoleId,
            CastRole = new CastRole
            {
                Id = castInMovie.CastRole!.Id,
                Naming = castInMovie.CastRole.Naming
            },
            PersonId = castInMovie.PersonId,
            Persons = castInMovie.Persons,
            MovieDetailsId = castInMovie.MovieDetailsId,
            MovieDetails = castInMovie.MovieDetails
        });
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetCastInMovie",
            new {id = castInMovie.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, castInMovie);
    }

    // DELETE: api/CastInMovies/5
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