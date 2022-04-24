#nullable disable
using App.Contracts.DAL;
using App.Domain.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly IAppUOW _uow;

    public SubscriptionsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/Subscriptions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetSubscriptions()
    {
        var res = (await _uow.Subscription.GetAllAsync())
            .Select(s => new SubscriptionDto
            {
                Id = s.Id,
                Naming = s.Naming,
                Description = s.Description,
                Price = s.Price,
                AppUserId = s.AppUserId,
                AppUser = s.AppUser
            })
            .ToList();
        return res;
    }

    // GET: api/Subscriptions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetSubscription(Guid id)
    {
        var subscription = await _uow.Subscription.FirstOrDefaultAsync(id);

        if (subscription == null) return NotFound();

        return subscription;
    }

    // PUT: api/Subscriptions/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubscription(Guid id, SubscriptionDto subscription)
    {
        if (id != subscription.Id) return BadRequest();

        var subscriptionFromDb = await _uow.Subscription.FirstOrDefaultAsync(id);
        if (subscriptionFromDb == null) return NotFound();

        try
        {
            subscriptionFromDb.Naming.SetTranslation(subscription.Naming);
            subscriptionFromDb.Description.SetTranslation(subscription.Description);
            _uow.Subscription.Update(subscriptionFromDb);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await SubscriptionExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Subscriptions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
    {
        _uow.Subscription.Add(subscription);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetSubscription", new {id = subscription.Id}, subscription);
    }

    // DELETE: api/Subscriptions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        await _uow.Subscription.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> SubscriptionExists(Guid id)
    {
        return await _uow.Subscription.ExistsAsync(id);
    }
}