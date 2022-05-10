#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class UserRatingsController : ControllerBase
{
    private readonly IAppBll _bll;

    public UserRatingsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/UserRatings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRatingDto>>> GetUserRatings()
    {
        var res = (await _bll.UserRating.GetAllAsync())
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
        var userRating = await _bll.UserRating.FirstOrDefaultAsync(id);

        if (userRating == null) return NotFound();

        return userRating;
    }

    // PUT: api/UserRatings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserRating(Guid id, UserRatingDto userRating)
    {
        if (id != userRating.Id) return BadRequest();

        var userRatingsFromDb = await _bll.UserRating.FirstOrDefaultAsync(id);
        if (userRatingsFromDb == null) return NotFound();

        try
        {
            userRatingsFromDb.Comment.SetTranslation(userRating.Comment);
            _bll.UserRating.Update(userRatingsFromDb);
            await _bll.SaveChangesAsync();
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
        _bll.UserRating.Add(userRating);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetUserRating", new {id = userRating.Id}, userRating);
    }

    // DELETE: api/UserRatings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserRating(Guid id)
    {
        await _bll.UserRating.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> UserRatingExists(Guid id)
    {
        return await _bll.UserRating.ExistsAsync(id);
    }
}