#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  Subscription entities.
///     Subscription entities meant for storage of subscription plans.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,moderator,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SubscriptionsController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of SubscriptionsController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public SubscriptionsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/Subscriptions
    /// <summary>
    ///     Method returns list of all Subscription entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of Subscription entities.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Subscription>), 200)]
    [HttpGet]
    public async Task<IEnumerable<Subscription>> GetUserSubscription()
    {
        return await _public.Subscription.GetAllAsync();
    }

    // GET: api/Subscriptions/5
    /// <summary>
    ///     Method returns one exact Subscription entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: Subscription entity Id</param>
    /// <returns>Subscription class entity</returns>
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
    /// <summary>
    ///     For admins and moderators only. Method edits Subscription entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: Subscription entity id.</param>
    /// <param name="subscription">Updated Subscription entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    /// <summary>
    ///     For admins and moderators only. Method adds new Subscription entity to API database
    /// </summary>
    /// <param name="subscription">Subscription class entity to add</param>
    /// <returns>Added Subscription entity with it's id </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Subscription), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
    {
        subscription.Id = Guid.NewGuid();
        _public.Subscription.Add(subscription);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetSubscription",
            new { id = subscription.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, subscription);
    }

    // DELETE: api/Subscriptions/5
    /// <summary>
    ///     For admins and moderators only. Deletes Subscription entity found by given id.
    /// </summary>
    /// <param name="id">Subscription entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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