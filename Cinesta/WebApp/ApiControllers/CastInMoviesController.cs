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
using App.Domain.Cast;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastInMoviesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public CastInMoviesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/CastInMovies
        [HttpGet]
        public async Task<IEnumerable<CastInMovie>> GetCastInMovies()
        {
            return await _uow.CastInMovie.GetAllAsync();
        }

        // GET: api/CastInMovies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CastInMovie>> GetCastInMovie(Guid id)
        {
            var castInMovie = await _uow.CastInMovie.FirstOrDefaultAsync(id);

            if (castInMovie == null)
            {
                return NotFound();
            }

            return castInMovie;
        }

        // PUT: api/CastInMovies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCastInMovie(Guid id, CastInMovie castInMovie)
        {
            if (id != castInMovie.Id)
            {
                return BadRequest();
            }

            try
            {
                _uow.CastInMovie.Update(castInMovie);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CastInMovieExists(id))
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

        // POST: api/CastInMovies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CastInMovie>> PostCastInMovie(CastInMovie castInMovie)
        {
            _uow.CastInMovie.Add(castInMovie);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetCastInMovie", new { id = castInMovie.Id }, castInMovie);
        }

        // DELETE: api/CastInMovies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCastInMovie(Guid id)
        {
            await _uow.CastInMovie.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CastInMovieExists(Guid id)
        {
            return await _uow.CastInMovie.ExistsAsync(id);
        }
    }
}
