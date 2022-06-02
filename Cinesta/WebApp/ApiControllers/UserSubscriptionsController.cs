#nullable disable
using App.Contracts.BLL;
using App.Contracts.Public;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
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
    private readonly IAppBll _bll;

    /// <summary>
    ///     Constructor of UserSubscriptionsController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public UserSubscriptionsController(IAppPublic appPublic, IAppBll bll)
    {
        _public = appPublic;
        _bll = bll;
    }

    // GET: api/UserSubscriptions
    /// <summary>
    ///     Method returns current user UserSubscription entity stored in API database.
    /// </summary>
    /// <returns>Generated from UserSubscription entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserSubscription), 200)]
    [SwaggerResponseExample(200, typeof(GetUserSubscriptionExample))]
    [HttpGet]
    public async Task<UserSubscription> GetUserSubscriptionByUserId(string culture)
    {
        var res = await _bll.UserSubscription.IncludeGetByUserIdAsync(User.GetUserId());
        if (res == null) return null;
        return new App.Public.DTO.v1.UserSubscription
        {
            Id = res.Id,
            AppUserId = res.AppUserId,
            AppUser = new AppUser
            {
                Id = res.Id,
                Name = res.AppUser!.Name,
                Surname = res.AppUser.Surname,
                PersonId = res.AppUser.PersonId,
            },
            SubscriptionId = res.SubscriptionId,
            Subscription = new Subscription
            {
                Id = res.Subscription!.Id,
                Naming = res.Subscription.Naming.Translate(culture)!,
                Description = res.Subscription.Description.Translate(culture)!,
                ProfilesCount = res.Subscription.ProfilesCount,
                Price = res.Subscription.ProfilesCount
            },
            ExpirationDateTime = res.ExpirationDateTime
        };
    }

    // POST: api/UserSubscriptions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     Method adds new UserSubscription entity for current user to API database. Only one subscription allowed.
    /// </summary>
    /// <param name="subscription">Subscription class entity id to link with user</param>
    /// <returns>Generated from UserSubscription entity object </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(UserSubscription), typeof(PostUserSubscriptionExample))]
    [SwaggerResponseExample(201, typeof(PostUserSubscriptionExample))]
    [HttpPost]
    public async Task<ActionResult<UserSubscription>> PostUserSubscription(Subscription subscription, string culture)
    {
        if (await GetUserSubscriptionByUserId(culture) != null) return BadRequest();
        var userSubscription = new UserSubscription
        {
            SubscriptionId = subscription.Id,
            Id = Guid.NewGuid(),
            AppUserId = User.GetUserId(),
            ExpirationDateTime = DateTime.UtcNow.AddMonths(1)
        };
        _public.UserSubscription.Add(userSubscription);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetUserSubscriptionByUserId",
            new { id = userSubscription.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() },
            userSubscription);
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