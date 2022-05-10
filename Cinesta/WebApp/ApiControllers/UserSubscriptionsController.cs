#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class UserSubscriptionsController : ControllerBase
{
    private readonly IAppBll _bll;

    public UserSubscriptionsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/UserSubscriptions
    [HttpGet]
    public async Task<IEnumerable<UserSubscription>> GetUserSubscriptions()
    {
        return await _bll.UserSubscription.GetAllAsync();
    }

    // GET: api/UserSubscriptions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserSubscription>> GetUserSubscription(Guid id)
    {
        var userSubscription = await _bll.UserSubscription.FirstOrDefaultAsync(id);

        if (userSubscription == null) return NotFound();

        return userSubscription;
    }

    // POST: api/UserSubscriptions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserSubscription>> PostUserSubscription(UserSubscription userSubscription)
    {
        _bll.UserSubscription.Add(userSubscription);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetUserSubscription", new {id = userSubscription.Id}, userSubscription);
    }

    // DELETE: api/UserSubscriptions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserSubscription(Guid id)
    {
        await _bll.UserSubscription.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> UserSubscriptionExists(Guid id)
    {
        return await _bll.UserSubscription.ExistsAsync(id);
    }
}