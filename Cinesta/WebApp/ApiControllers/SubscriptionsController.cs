#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SubscriptionsController : ControllerBase
{
    private readonly IAppBll _bll;

    public SubscriptionsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/Subscriptions
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Subscription>), 200)]
    [HttpGet]
    public async Task<IEnumerable<Subscription>> GetSubscriptions()
    {
        return await _bll.Subscription.GetAllAsync();
    }

    // GET: api/Subscriptions/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Subscription), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetSubscription(Guid id)
    {
        var subscription = await _bll.Subscription.FirstOrDefaultAsync(id);

        if (subscription == null) return NotFound();

        return subscription;
    }

    // PUT: api/Subscriptions/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutSubscription(Guid id, Subscription subscription)
    {
        if (id != subscription.Id) return BadRequest();

        var subscriptionFromDb = await _bll.Subscription.FirstOrDefaultAsync(id);
        if (subscriptionFromDb == null) return NotFound();

        try
        {
            subscriptionFromDb.Naming.SetTranslation(subscription.Naming);
            subscriptionFromDb.Description.SetTranslation(subscription.Description);
            _bll.Subscription.Update(subscriptionFromDb);
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await SubscriptionExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Subscriptions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Subscription),201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
    {
        _bll.Subscription.Add(subscription);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetSubscription", new {id = subscription.Id,  version = HttpContext.GetRequestedApiVersion()!.ToString()}, subscription);
    }

    // DELETE: api/Subscriptions/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        await _bll.Subscription.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> SubscriptionExists(Guid id)
    {
        return await _bll.Subscription.ExistsAsync(id);
    }
}