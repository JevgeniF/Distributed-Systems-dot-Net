#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
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
    private readonly IAppPublic _public;

    public UserRatingsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/UserRatings
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<UserRating>), 200)]
    [HttpGet]
    public async Task<IEnumerable<UserRating>> GetUserRatings()
    {
        return await _public.UserRating.GetAllAsync();
    }

    // GET: api/UserRatings/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserRating), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserRating>> GetUserRating(Guid id)
    {
        var userRating = await _public.UserRating.FirstOrDefaultAsync(id);

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

        var userRatingsFromDb = await _public.UserRating.FirstOrDefaultAsync(id);
        if (userRatingsFromDb == null) return NotFound();

        try
        {
            userRatingsFromDb.Comment.SetTranslation(userRating.Comment);
            _public.UserRating.Update(userRatingsFromDb);
            await _public.SaveChangesAsync();
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
    [ProducesResponseType(typeof(UserRating), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<UserRating>> PostUserRating(UserRating userRating)
    {
        userRating.Id = Guid.NewGuid();
        _public.UserRating.Add(userRating);
        await _public.SaveChangesAsync();

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
        await _public.UserRating.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> UserRatingExists(Guid id)
    {
        return await _public.UserRating.ExistsAsync(id);
    }
}