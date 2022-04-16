#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Movie;
using WebApp.DTO;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MovieDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieDetails()
        {
            var res = (await _context.MovieDetails.ToListAsync())
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
            var movieDetails = await _context.MovieDetails.FindAsync(id);

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
            
            var movieDetailsFromDb = await _context.MovieDetails.FindAsync(id);
            if (movieDetailsFromDb == null)
            {
                return NotFound();
            }
            
            movieDetailsFromDb.Title.SetTranslation(movieDetails.Title);
            movieDetailsFromDb.Description.SetTranslation(movieDetails.Description);

            _context.Entry(movieDetailsFromDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieDetailsExists(id))
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
            _context.MovieDetails.Add(movieDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieDetails", new { id = movieDetails.Id }, movieDetails);
        }

        // DELETE: api/MovieDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieDetails(Guid id)
        {
            var movieDetails = await _context.MovieDetails.FindAsync(id);
            if (movieDetails == null)
            {
                return NotFound();
            }

            _context.MovieDetails.Remove(movieDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieDetailsExists(Guid id)
        {
            return _context.MovieDetails.Any(e => e.Id == id);
        }
    }
}
