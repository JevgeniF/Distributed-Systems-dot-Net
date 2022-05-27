#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.UserSubscriptions;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  UserSubscription entities.
///     UserSubscription entity meant for between-connection of AppUser and Subscription.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserSubscriptionsController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of UserSubscriptionsController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public UserSubscriptionsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/UserSubscriptions
    /// <summary>
    ///     Method returns current user UserSubscription entity stored in API database.
    /// </summary>
    /// <returns>Generated from UserSubscription entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [SwaggerResponseExample(200, typeof(GetUserSubscriptionExample))]
    [HttpGet]
    public async Task<object> GetUserSubscriptionByUserId()
    {
        var res = await _public.UserSubscription.IncludeGetByUserIdAsync(User.GetUserId());
        if (res == null) return null;
        return new
        {
            res.Id,
            AppUser = new
            {
                res.AppUserId,
                res.AppUser!.Name,
                res.AppUser.Surname
            },
            Subscription = new
            {
                res.SubscriptionId,
                res.Subscription!.Naming,
                res.Subscription.Description,
                res.Subscription.ProfilesCount,
                res.Subscription.Price
            },
            res.ExpirationDateTime
        };
    }

    // POST: api/UserSubscriptions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     Method adds new UserSubscription entity for current user to API database. Only one subscription allowed.
    /// </summary>
    /// <param name="userSubscription">UserSubscription class entity to add</param>
    /// <returns>Generated from UserSubscription entity object </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(UserSubscription), typeof(PostUserSubscriptionExample))]
    [SwaggerResponseExample(201, typeof(PostUserSubscriptionExample))]
    [HttpPost]
    public async Task<ActionResult<UserSubscription>> PostUserSubscription(UserSubscription userSubscription)
    {
        if (await GetUserSubscriptionByUserId() != null) return BadRequest();
        userSubscription.Id = Guid.NewGuid();
        userSubscription.AppUserId = User.GetUserId();
        _public.UserSubscription.Add(userSubscription);
        await _public.SaveChangesAsync();

        var res = new
        {
            userSubscription.Id,
            AppUser = new
            {
                userSubscription.AppUserId,
                userSubscription.AppUser!.Name,
                userSubscription.AppUser.Surname
            },
            Subscription = new
            {
                userSubscription.SubscriptionId,
                userSubscription.Subscription!.Naming,
                userSubscription.Subscription.Description,
                userSubscription.Subscription.ProfilesCount,
                userSubscription.Subscription.Price
            },
            userSubscription.ExpirationDateTime
        };

        return CreatedAtAction("GetUserSubscriptionByUserId",
            new { id = userSubscription.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() },
            res);
    }

    // DELETE: api/UserSubscriptions/5
    /// <summary>
    ///     Deletes UserSubscription entity found by given id.
    /// </summary>
    /// <param name="id">UserSubscription entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserSubscription(Guid id)
    {
        var subscriptionInDb = await _public.UserSubscription.FirstOrDefaultAsync(id);
        if (subscriptionInDb == null) return NotFound();
        if (subscriptionInDb.AppUserId != User.GetUserId()) return BadRequest();
        await _public.UserSubscription.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }
}