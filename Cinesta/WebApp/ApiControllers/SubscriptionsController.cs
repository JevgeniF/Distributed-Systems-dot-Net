#nullable disable
using System.Security;
using App.Contracts.BLL;
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Domain;
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
    private readonly IAppBll _bll;

    /// <summary>
    ///     Constructor of SubscriptionsController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public SubscriptionsController(IAppPublic appPublic, IAppBll bll)
    {
        _public = appPublic;
        _bll = bll;
    }

    // GET: api/Subscriptions
    /// <summary>
    ///     Method returns list of all Subscription entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of Subscription entities.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [HttpGet]
    public async Task<IEnumerable<object>> GetUserSubscription(string culture)
    {
        var subscriptions = await _bll.Subscription.GetAllAsync();
        return subscriptions.Select(s => new App.Public.DTO.v1.Subscription {
            Id = s.Id,
            Naming = s.Naming.Translate(culture)!,
            Description = s.Description.Translate(culture)!,
            ProfilesCount = s.ProfilesCount,
            Price = s.Price
            
        });
    }

    // GET: api/Subscriptions/5
    /// <summary>
    ///     Method returns one exact Subscription entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: Subscription entity Id</param>
    /// <returns>Subscription class entity</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetSubscription(Guid id, string culture)
    {
        var subscription = await _bll.Subscription.FirstOrDefaultAsync(id);

        if (subscription == null) return NotFound();

        return new App.Public.DTO.v1.Subscription
        {
            Id = subscription.Id,
            Naming = subscription.Naming.Translate(culture)!,
            Description = subscription.Description.Translate(culture)!,
            ProfilesCount = subscription.ProfilesCount,
            Price = subscription.Price

        };
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
    public async Task<IActionResult> PutSubscription(Guid id, Subscription subscription, string culture)
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
    public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription, string culture)
    {
        subscription.Id = Guid.NewGuid();
        subscription.Naming = new LangStr(subscription.Naming, culture);
        subscription.Description = new LangStr(subscription.Description, culture);
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
    public async Task<IActionResult> DeleteSubscription(Guid id, string culture)
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