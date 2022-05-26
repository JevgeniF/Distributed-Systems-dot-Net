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
public class MovieGenresController : ControllerBase
{
    private readonly IAppPublic _public;

    public MovieGenresController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/MovieGenres
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponseExample(200, typeof(GetListMovieGenresExample))]
    [ProducesResponseType(typeof(IEnumerable<MovieGenre>), 200)]
    [HttpGet]
    public async Task<IEnumerable<object>> GetMovieGenres()
    {
        return (await _public.MovieGenre.IncludeGetAllAsync())
            .Select(m => new
            {
                m.Id,
                MovieDetails = new
                {
                    Id = m.MovieDetailsId,
                    m.MovieDetails!.Title
                },
                Genre = new
                {
                    Id = m.GenreId,
                    m.Genre!.Naming
                }
            });
    }

    // GET: api/MovieGenres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetMovieGenresExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetMovieGenre(Guid id)
    {
        var movieGenre = await _public.MovieGenre.FirstOrDefaultAsync(id);

        if (movieGenre == null) return NotFound();

        return new
        {
            movieGenre.Id,
            MovieDetails = new
            {
                Id = movieGenre.MovieDetailsId,
                movieGenre.MovieDetails!.Title
            },
            Genre = new
            {
                Id = movieGenre.GenreId,
                movieGenre.Genre!.Naming
            }
                
        };
    }

    // PUT: api/MovieGenres/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [SwaggerRequestExample(typeof(MovieGenre), typeof(PostMovieGenresExample))]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerResponseExample(201, typeof(PostMovieGenresExample))]
    [SwaggerRequestExample(typeof(MovieGenre), typeof(PostMovieGenresExample))]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            new {id = movieGenre.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, res);
    }

    // DELETE: api/MovieGenres/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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