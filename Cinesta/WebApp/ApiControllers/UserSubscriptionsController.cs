#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserSubscriptionsController : ControllerBase
{
    private readonly IAppPublic _public;

    public UserSubscriptionsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/UserSubscriptions
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<UserSubscription>), 200)]
    [HttpGet]
    public async Task<IEnumerable<UserSubscription>> GetUserSubscriptionsByUserId()
    {
        return await _public.UserSubscription.IncludeGetAllByUserIdAsync(User.GetUserId());
    }

    // GET: api/UserSubscriptions/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserSubscription), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserSubscription>> GetUserSubscription(Guid id)
    {
        var userSubscription = await _public.UserSubscription.FirstOrDefaultAsync(id);

        if (userSubscription == null) return NotFound();

        return userSubscription;
    }

    // POST: api/UserSubscriptions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserSubscription), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<UserSubscription>> PostUserSubscription(UserSubscription userSubscription)
    {
        userSubscription.Id = Guid.NewGuid();
        _public.UserSubscription.Add(userSubscription);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetUserSubscription",
            new {id = userSubscription.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()},
            userSubscription);
    }

    // DELETE: api/UserSubscriptions/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserSubscription(Guid id)
    {
        await _public.UserSubscription.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> UserSubscriptionExists(Guid id)
    {
        return await _public.UserSubscription.ExistsAsync(id);
    }
}