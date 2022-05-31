#nullable disable
using System.Globalization;
using App.Contracts.BLL;
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.MovieDetails;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  MovieDetails entities.
///     MovieDetails entities meant for storage of every movie details.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MovieDetailsController : ControllerBase
{
    private readonly IAppPublic _public;
    private readonly IAppBll _bll;

    /// <summary>
    ///     Constructor of MovieDetailsController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public MovieDetailsController(IAppPublic appPublic, IAppBll bll)
    {
        _public = appPublic;
        _bll = bll;
    }

    // GET: api/MovieDetails
    /// <summary>
    ///     Method returns list of all MovieDetails entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of generated from MovieDetails entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<MovieDetails>), 200)]
    [SwaggerResponseExample(200, typeof(GetListMovieDetailsExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetMovieDetails(string culture)
    {
        var res = await _bll.MovieDetails.IncludeGetAllAsync();
        return res.Select(m => new
        {
            m.Id,
            m.PosterUri,
            Title = m.Title.Translate(culture),
            m.Released,
            Description = m.Description.Translate(culture),
            AgeRating = new AgeRating
            {
                Id = m.AgeRatingId,
                Naming = m.AgeRating!.Naming,
                AllowedAge = m.AgeRating.AllowedAge
            },
            MovieType = new MovieType
            {
                Id = m.MovieTypeId,
                Naming = m.MovieType!.Naming.Translate(culture)!
            }
        });
    }

    // GET: api/MovieDetails/5
    /// <summary>
    ///     Method returns one exact MovieDetails entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieDetails entity Id</param>
    /// <returns>Generated from MovieDetails entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetMovieDetailsExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetMovieDetails(Guid id, string culture)
    {
        var movieDetails = await _bll.MovieDetails.FirstOrDefaultAsync(id);

        if (movieDetails == null) return NotFound();

        return new
        {
            movieDetails.Id,
            movieDetails.PosterUri,
            Title = movieDetails.Title.Translate(culture),
            movieDetails.Released,
            Description = movieDetails.Description.Translate(culture),
            AgeRating = new AgeRating
            {
                Id = movieDetails.AgeRatingId,
                Naming = movieDetails.AgeRating!.Naming,
                AllowedAge = movieDetails.AgeRating.AllowedAge
            },
            MovieType = new MovieType
            {
                Id = movieDetails.MovieTypeId,
                Naming = movieDetails.MovieType!.Naming.Translate(culture)!
            }
        };
    }

    // PUT: api/MovieDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits MovieDetails entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: MovieDetails entity id.</param>
    /// <param name="movieDetails">Updated MovieDetails entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(MovieDetails), typeof(PostMovieDetailsExample))]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieDetails(Guid id, MovieDetails movieDetails)
    {
        if (id != movieDetails.Id) return BadRequest();

        var movieDetailsFromDb = await _bll.MovieDetails.FirstOrDefaultAsync(id);
        if (movieDetailsFromDb == null) return NotFound();

        try
        {
            movieDetailsFromDb.Title.SetTranslation(movieDetails.Title);
            movieDetailsFromDb.Description.SetTranslation(movieDetails.Description);
            _bll.MovieDetails.Update(movieDetailsFromDb);
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieDetailsExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/MovieDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method adds new MovieDetails entity to API database
    /// </summary>
    /// <param name="movieDetails">MovieDetails class entity to add</param>
    /// <returns>Generated from MovieDetails entity object </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(MovieDetails), typeof(PostMovieDetailsExample))]
    [SwaggerResponseExample(201, typeof(PostMovieDetailsExample))]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MovieDetails>> PostMovieDetails(MovieDetails movieDetails)
    {
        movieDetails.Id = Guid.NewGuid();
        _public.MovieDetails.Add(movieDetails);
        await _public.SaveChangesAsync();

        var res = new
        {
            movieDetails.Id,
            movieDetails.AgeRatingId,
            movieDetails.MovieTypeId
        };

        return CreatedAtAction("GetMovieDetails",
            new { id = movieDetails.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, res);
    }

    // DELETE: api/MovieDetails/5
    /// <summary>
    ///     For admins and moderators only. Deletes MovieDetails entity found by given id.
    /// </summary>
    /// <param name="id">MovieDetails entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieDetails(Guid id)
    {
        await _public.MovieDetails.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieDetailsExists(Guid id)
    {
        return await _public.MovieDetails.ExistsAsync(id);
    }
}