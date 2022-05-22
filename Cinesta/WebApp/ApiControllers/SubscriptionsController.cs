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
public class SubscriptionsController : ControllerBase
{
    private readonly IAppPublic _public;

    public SubscriptionsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/Subscriptions
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Subscription>), 200)]
    [HttpGet]
    public async Task<IEnumerable<Subscription>> GetSubscriptions()
    {
        return await _public.Subscription.GetAllAsync();
    }

    // GET: api/Subscriptions/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Subscription), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetSubscription(Guid id)
    {
        var subscription = await _public.Subscription.FirstOrDefaultAsync(id);

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

        var subscriptionFromDb = await _public.Subscription.FirstOrDefaultAsync(id);
        if (subscriptionFromDb == null) return NotFound();

        try
        {
            subscriptionFromDb.Naming.SetTranslation(subscription.Naming);
            subscriptionFromDb.Description.SetTranslation(subscription.Description);
            _public.Subscription.Update(subscriptionFromDb);
            await _public.SaveChangesAsync();
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
    [ProducesResponseType(typeof(Subscription), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
    {
        subscription.Id = Guid.NewGuid();
        _public.Subscription.Add(subscription);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetSubscription",
            new {id = subscription.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, subscription);
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
        await _public.Subscription.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> SubscriptionExists(Guid id)
    {
        return await _public.Subscription.ExistsAsync(id);
    }
}