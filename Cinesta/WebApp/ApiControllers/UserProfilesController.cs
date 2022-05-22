#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [ProducesResponseType(typeof(IEnumerable<UserProfile>), 200)]
    [HttpGet]
    public async Task<IEnumerable<UserProfile>> GetUserProfilesByUserId()
    {
        return await _public.UserProfile.IncludeGetAllByUserIdAsync(User.GetUserId());
    }

    // GET: api/UserProfiles/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserProfile), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserProfile>> GetUserProfile(Guid id)
    {
        var userProfile = await _public.UserProfile.FirstOrDefaultAsync(id);

        if (userProfile == null) return NotFound();

        return userProfile;
    }

    // PUT: api/UserProfiles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserProfile(Guid id, UserProfile userProfile)
    {
        if (id != userProfile.Id) return BadRequest();

        try
        {
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
    [ProducesResponseType(typeof(UserProfile), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
    {
        userProfile.Id = Guid.NewGuid();
        _public.UserProfile.Add(userProfile);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetUserProfile",
            new {id = userProfile.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, userProfile);
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