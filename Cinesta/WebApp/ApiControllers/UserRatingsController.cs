#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples;

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
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [SwaggerResponseExample(200, typeof(GetListUserRatingsExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetUserRatings()
    {
        return (await _public.UserRating.IncludeGetAllAsync())
            .Select(u => new
            {
                u.Id,
                AppUser = new
                {
                    u.AppUserId,
                    u.AppUser!.Name,
                    u.AppUser.Surname
                },
                MovieDetails = new
                {
                    u.MovieDetailsId,
                    u.MovieDetails!.Title
                },
                u.Rating,
                u.Comment
            });
    }

    // GET: api/UserRatings/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetUserRatingExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetUserRating(Guid id)
    {
        var userRating = await _public.UserRating.FirstOrDefaultAsync(id);

        if (userRating == null) return NotFound();

        return new
        {
            userRating.Id,
            AppUser = new
            {
                userRating.AppUserId,
                userRating.AppUser!.Name,
                userRating.AppUser.Surname
            },
            MovieDetails = new
            {
                userRating.MovieDetailsId,
                userRating.MovieDetails!.Title
            },
            userRating.Rating,
            userRating.Comment
        };
    }

    // PUT: api/UserRatings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(UserRating), typeof(PostUserRatingExample))]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserRating(Guid id, UserRating userRating)
    {
        if (id != userRating.Id) return BadRequest();

        var userRatingsFromDb = await _public.UserRating.FirstOrDefaultAsync(id);
        if (userRatingsFromDb == null) return NotFound();

        try
        {
            userRating.AppUserId = User.GetUserId();
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
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(UserRating), typeof(PostUserRatingExample))]
    [SwaggerResponseExample(201, typeof(PostUserRatingExample))]
    [HttpPost]
    public async Task<ActionResult<object>> PostUserRating(UserRating userRating)
    {
        userRating.Id = Guid.NewGuid();
        _public.UserRating.Add(userRating);
        await _public.SaveChangesAsync();

        var res = new
        {
            userRating.Id,
            userRating.AppUserId,
            userRating.MovieDetailsId,
            userRating.Rating,
            userRating.Comment
        };
        return CreatedAtAction("GetUserRating",
            new {id = userRating.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, res);
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