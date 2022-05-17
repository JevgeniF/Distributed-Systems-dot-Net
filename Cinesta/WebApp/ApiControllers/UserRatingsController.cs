#nullable disable
using App.Contracts.BLL;
using App.Public.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserRatingsController : ControllerBase
{
    private readonly IAppBll _bll;

    public UserRatingsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/UserRatings
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<UserRating>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRating>>> GetUserRatings()
    {
        var res = (await _bll.UserRating.GetAllAsync())
            .Select(u => new UserRating
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.BLL.DTO.UserRating), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.BLL.DTO.UserRating>> GetUserRating(Guid id)
    {
        var userRating = await _bll.UserRating.FirstOrDefaultAsync(id);

        if (userRating == null) return NotFound();

        return userRating;
    }

    // PUT: api/UserRatings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserRating(Guid id, UserRating userRating)
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.BLL.DTO.UserRating), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<App.BLL.DTO.UserRating>> PostUserRating(App.BLL.DTO.UserRating userRating)
    {
        _bll.UserRating.Add(userRating);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetUserRating",
            new {id = userRating.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, userRating);
    }

    // DELETE: api/UserRatings/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
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