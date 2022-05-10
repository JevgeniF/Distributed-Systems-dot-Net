#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class UserProfilesController : ControllerBase
{
    private readonly IAppBll _bll;

    public UserProfilesController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/UserProfiles
    [HttpGet]
    public async Task<IEnumerable<UserProfile>> GetUserProfiles()
    {
        return await _bll.UserProfile.GetAllAsync();
    }

    // GET: api/UserProfiles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserProfile>> GetUserProfile(Guid id)
    {
        var userProfile = await _bll.UserProfile.FirstOrDefaultAsync(id);

        if (userProfile == null) return NotFound();

        return userProfile;
    }

    // PUT: api/UserProfiles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserProfile(Guid id, UserProfile userProfile)
    {
        if (id != userProfile.Id) return BadRequest();

        try
        {
            _bll.UserProfile.Update(userProfile);
            await _bll.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
    {
        _bll.UserProfile.Add(userProfile);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetUserProfile", new {id = userProfile.Id}, userProfile);
    }

    // DELETE: api/UserProfiles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserProfile(Guid id)
    {
        await _bll.UserProfile.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private Task<bool> UserProfileExists(Guid id)
    {
        return _bll.UserProfile.ExistsAsync(id);
    }
}