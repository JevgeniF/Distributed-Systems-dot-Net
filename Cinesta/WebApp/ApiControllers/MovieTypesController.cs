#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.MovieStandardDetails;
using WebApp.DTO;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MovieTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieTypeDto>>> GetMovieTypes()
        {
            var res = (await _context.MovieTypes.ToListAsync())
                .Select(m => new MovieTypeDto()
                {
                    Id = m.Id,
                    Naming = m.Naming
                })
                .ToList();
            return res;
        }

        // GET: api/MovieTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieType>> GetMovieType(Guid id)
        {
            var movieType = await _context.MovieTypes.FindAsync(id);

            if (movieType == null)
            {
                return NotFound();
            }

            return movieType;
        }

        // PUT: api/MovieTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieType(Guid id, MovieTypeDto movieType)
        {
            if (id != movieType.Id)
            {
                return BadRequest();
            }

            var movieTypeFromDb = await _context.MovieTypes.FindAsync(id);
            if (movieTypeFromDb == null)
            {
                return NotFound();
            }
            
            movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
            
            _context.Entry(movieTypeFromDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieTypeExists(id))
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

        // POST: api/MovieTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieType>> PostMovieType(MovieType movieType)
        {
            _context.MovieTypes.Add(movieType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieType", new { id = movieType.Id }, movieType);
        }

        // DELETE: api/MovieTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieType(Guid id)
        {
            var movieType = await _context.MovieTypes.FindAsync(id);
            if (movieType == null)
            {
                return NotFound();
            }

            _context.MovieTypes.Remove(movieType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieTypeExists(Guid id)
        {
            return _context.MovieTypes.Any(e => e.Id == id);
        }
    }
}
