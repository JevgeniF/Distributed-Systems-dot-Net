#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Movie;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDBScoresController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public MovieDBScoresController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/MovieDBScores
        [HttpGet]
        public async Task<IEnumerable<MovieDbScore>> GetMovieDbScores()
        {
            return await _uow.MovieDbScore.GetAllAsync();
        }

        // GET: api/MovieDBScores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDbScore>> GetMovieDbScore(Guid id)
        {
            var movieDbScore = await _uow.MovieDbScore.FirstOrDefaultAsync(id);

            if (movieDbScore == null)
            {
                return NotFound();
            }

            return movieDbScore;
        }

        // PUT: api/MovieDBScores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieDbScore(Guid id, MovieDbScore movieDbScore)
        {
            if (id != movieDbScore.Id)
            {
                return BadRequest();
            }

            try
            {
                _uow.MovieDbScore.Update(movieDbScore);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MovieDbScoreExists(id))
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

        // POST: api/MovieDBScores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDbScore>> PostMovieDbScore(MovieDbScore movieDbScore)
        {
            _uow.MovieDbScore.Add(movieDbScore);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetMovieDbScore", new { id = movieDbScore.Id }, movieDbScore);
        }

        // DELETE: api/MovieDBScores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieDbScore(Guid id)
        {
            await _uow.MovieDbScore.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> MovieDbScoreExists(Guid id)
        {
            return await _uow.MovieDbScore.ExistsAsync(id);
        }
    }
}
