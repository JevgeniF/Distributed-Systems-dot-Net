#nullable disable
using App.Contracts.DAL;
using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class UserRatingsController : ControllerBase
{
    private readonly IAppUOW _uow;

    public UserRatingsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/UserRatings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRatingDto>>> GetUserRatings()
    {
        var res = (await _uow.UserRating.GetAllAsync())
            .Select(u => new UserRatingDto
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
        var userRating = await _uow.UserRating.FirstOrDefaultAsync(id);

        if (userRating == null) return NotFound();

        return userRating;
    }

    // PUT: api/UserRatings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserRating(Guid id, UserRatingDto userRating)
    {
        if (id != userRating.Id) return BadRequest();

        var userRatingsFromDb = await _uow.UserRating.FirstOrDefaultAsync(id);
        if (userRatingsFromDb == null) return NotFound();

        try
        {
            userRatingsFromDb.Comment.SetTranslation(userRating.Comment);
            _uow.UserRating.Update(userRatingsFromDb);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserRatingExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/UserRatings
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserRating>> PostUserRating(UserRating userRating)
    {
        _uow.UserRating.Add(userRating);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetUserRating", new {id = userRating.Id}, userRating);
    }

    // DELETE: api/UserRatings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserRating(Guid id)
    {
        await _uow.UserRating.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> UserRatingExists(Guid id)
    {
        return await _uow.UserRating.ExistsAsync(id);
    }
}