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
    public class UserRatingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserRatingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserRatings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRatingDto>>> GetUserRatings()
        {
            var res = (await _context.UserRatings.ToListAsync())
                .Select(u => new UserRatingDto()
                {
                    Id = u.Id,
                    Rating = u.Rating,
                    Comment = u.Comment,
                    AppUserId = u.AppUserId,
                    AppUser = u.AppUser,
                    MovieDetailsId = u.MovieDetailsId,
                    MovieDetails = u.MovieDetails
                })
                .ToList();
            return res;
        }

        // GET: api/UserRatings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRating>> GetUserRating(Guid id)
        {
            var userRating = await _context.UserRatings.FindAsync(id);

            if (userRating == null)
            {
                return NotFound();
            }

            return userRating;
        }

        // PUT: api/UserRatings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRating(Guid id, UserRatingDto userRating)
        {
            if (id != userRating.Id)
            {
                return BadRequest();
            }

            var userRatingsFromDb = await _context.UserRatings.FindAsync(id);
            if (userRatingsFromDb == null)
            {
                return NotFound();
            }

            userRatingsFromDb.Comment.SetTranslation(userRating.Comment);
            _context.Entry(userRatingsFromDb).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRatingExists(id))
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

        // POST: api/UserRatings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserRating>> PostUserRating(UserRating userRating)
        {
            _context.UserRatings.Add(userRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserRating", new { id = userRating.Id }, userRating);
        }

        // DELETE: api/UserRatings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRating(Guid id)
        {
            var userRating = await _context.UserRatings.FindAsync(id);
            if (userRating == null)
            {
                return NotFound();
            }

            _context.UserRatings.Remove(userRating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRatingExists(Guid id)
        {
            return _context.UserRatings.Any(e => e.Id == id);
        }
    }
}
