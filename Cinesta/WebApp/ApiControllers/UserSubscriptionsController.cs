#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples;
using Subscription = App.BLL.DTO.Subscription;

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
    [ProducesResponseType(typeof(object), 200)]
    [SwaggerResponseExample(200, typeof(GetUserSubscriptionExample))]
    [HttpGet]
    public async Task<object> GetUserSubscriptionByUserId()
    {
        var res = await _public.UserSubscription.IncludeGetByUserIdAsync(User.GetUserId());
        if (res == null) return new {};
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(UserSubscription), typeof(PostUserSubscriptionExample))]
    [SwaggerResponseExample(201, typeof(PostUserSubscriptionExample))]
    [HttpPost]
    public async Task<ActionResult<UserSubscription>> PostUserSubscription(UserSubscription userSubscription)
    {
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
            new {id = userSubscription.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()},
            res);
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
}