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
public class UserProfilesController : ControllerBase
{
    private readonly IAppPublic _public;

    public UserProfilesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/UserProfiles
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [SwaggerResponseExample(200, typeof(GetListUserProfilesExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetUserProfilesByUserId()
    {
        return (await _public.UserProfile.IncludeGetAllByUserIdAsync(User.GetUserId()))
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Age,
                u.AppUserId
            });
    }

    // GET: api/UserProfiles/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetUserProfileExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetUserProfile(Guid id)
    {
        var userProfile = await _public.UserProfile.FirstOrDefaultAsync(id);

        if (userProfile == null || userProfile.AppUserId != User.GetUserId()) return NotFound();

        return new
        {
            userProfile.Id,
            userProfile.Name,
            userProfile.Age,
            userProfile.AppUserId
        };
    }

    // PUT: api/UserProfiles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(UserProfile), typeof(PostUserProfileExample))]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserProfile(Guid id, UserProfile userProfile)
    {
        if (id != userProfile.Id) return BadRequest();

        try
        {
            userProfile.AppUserId = User.GetUserId();
            _public.UserProfile.Update(userProfile);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserProfileExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/UserProfiles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(UserProfile), typeof(PostUserProfileExample))]
    [SwaggerResponseExample(201, typeof(PostUserProfileExample))]
    [HttpPost]
    public async Task<ActionResult<object>> PostUserProfile(UserProfile userProfile)
    {
        userProfile.Id = Guid.NewGuid();
        userProfile.AppUserId = User.GetUserId();
        _public.UserProfile.Add(userProfile);
        await _public.SaveChangesAsync();

        var res = new
        {
            userProfile.Id,
            userProfile.IconUri,
            userProfile.Name,
            userProfile.Age,
            userProfile.AppUserId
        };

        return CreatedAtAction("GetUserProfile",
            new {id = userProfile.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, res);
    }

    // DELETE: api/UserProfiles/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserProfile(Guid id)
    {
        await _public.UserProfile.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private Task<bool> UserProfileExists(Guid id)
    {
        return _public.UserProfile.ExistsAsync(id);
    }
}