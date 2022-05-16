#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;
using MovieDetails = App.Public.DTO.v1.MovieDetails;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MovieDetailsController : ControllerBase
{
    private readonly IAppBll _bll;

    public MovieDetailsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/MovieDetails
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<MovieDetails>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDetails>>> GetMovieDetails()
    {
        var res = (await _bll.MovieDetails.GetAllAsync())
            .Select(m => new MovieDetails
            {
                Id = m.Id,
                PosterUri = m.PosterUri,
                Title = m.Title,
                Released = m.Released,
                Description = m.Description,
                AgeRatingId = m.AgeRatingId,
                AgeRating = m.AgeRating,
                MovieTypeId = m.MovieTypeId,
                MovieType = m.MovieType,
                MovieDbScores = m.MovieDbScores,
                Genres = m.MovieGenres,
                Videos = m.Videos,
                UserRatings = m.UserRatings,
                CastInMovie = m.CastInMovie
            })
            .ToList();
        return res;
    }

    // GET: api/MovieDetails/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.BLL.DTO.MovieDetails), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.BLL.DTO.MovieDetails>> GetMovieDetails(Guid id)
    {
        var movieDetails = await _bll.MovieDetails.FirstOrDefaultAsync(id);

        if (movieDetails == null) return NotFound();

        return movieDetails;
    }

    // PUT: api/MovieDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.BLL.DTO.MovieDetails),201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<App.BLL.DTO.MovieDetails>> PostMovieDetails(App.BLL.DTO.MovieDetails movieDetails)
    {
        _bll.MovieDetails.Add(movieDetails);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetMovieDetails", new {id = movieDetails.Id,  version = HttpContext.GetRequestedApiVersion()!.ToString()}, movieDetails);
    }

    // DELETE: api/MovieDetails/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieDetails(Guid id)
    {
        await _bll.MovieDetails.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieDetailsExists(Guid id)
    {
        return await _bll.MovieDetails.ExistsAsync(id);
    }
}