#nullable disable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Movie;
using WebApp.DTO;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDetailsController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public MovieDetailsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/MovieDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieDetails()
        {
            var res = (await _uow.MovieDetails.GetAllAsync())
                .Select(m => new MovieDetailsDto()
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
                    Genres = m.Genres,
                    Videos = m.Videos,
                    UserRatings = m.UserRatings,
                    CastInMovie = m.CastInMovie
                })
                .ToList();
            return res;
        }

        // GET: api/MovieDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetails>> GetMovieDetails(Guid id)
        {
            var movieDetails = await _uow.MovieDetails.FirstOrDefaultAsync(id);

            if (movieDetails == null)
            {
                return NotFound();
            }

            return movieDetails;
        }

        // PUT: api/MovieDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieDetails(Guid id, MovieDetailsDto movieDetails)
        {
            if (id != movieDetails.Id)
            {
                return BadRequest();
            }
            
            var movieDetailsFromDb = await _uow.MovieDetails.FirstOrDefaultAsync(id);
            if (movieDetailsFromDb == null)
            {
                return NotFound();
            }

            try
            {
                movieDetailsFromDb.Title.SetTranslation(movieDetails.Title);
                movieDetailsFromDb.Description.SetTranslation(movieDetails.Description);
                _uow.MovieDetails.Update(movieDetailsFromDb);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MovieDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MovieDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDetails>> PostMovieDetails(MovieDetails movieDetails)
        {
            _uow.MovieDetails.Add(movieDetails);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetMovieDetails", new { id = movieDetails.Id }, movieDetails);
        }

        // DELETE: api/MovieDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieDetails(Guid id)
        {
            await _uow.MovieDetails.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> MovieDetailsExists(Guid id)
        {
            return await _uow.MovieDetails.ExistsAsync(id);
        }
    }
}
