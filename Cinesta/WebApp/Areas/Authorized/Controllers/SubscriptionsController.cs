#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class SubscriptionsController : Controller
{
    private readonly ILogger<SubscriptionsController> _logger;
    private readonly IAppPublic _public;

    public SubscriptionsController(IAppPublic appPublic, ILogger<SubscriptionsController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/Subscriptions
    public async Task<IActionResult> Index()
    {
        return View(await _public.Subscription.GetAllAsync());
    }

    // GET: Authorized/Subscriptions/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _public.Subscription.FirstOrDefaultAsync(id.Value);
        if (subscription == null) return NotFound();

        return View(subscription);
    }

    // GET: Authorized/Subscriptions/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authorized/Subscriptions/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Naming,Description,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        Subscription subscription)
    {
        if (ModelState.IsValid)
        {
            _public.Subscription.Add(subscription);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(subscription);
    }

    // GET: Authorized/Subscriptions/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _public.Subscription.FirstOrDefaultAsync(id.Value);
        if (subscription == null) return NotFound();
        return View(subscription);
    }

    // POST: Authorized/Subscriptions/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,Description,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        Subscription subscription)
    {
        if (id != subscription.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _public.Subscription.Update(subscription);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SubscriptionExists(subscription.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(subscription);
    }

    // GET: Authorized/Subscriptions/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _public.Subscription.FirstOrDefaultAsync(id.Value);
        if (subscription == null) return NotFound();

        return View(subscription);
    }

    // POST: Authorized/Subscriptions/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.Subscription.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> SubscriptionExists(Guid id)
    {
        return await _public.Subscription.ExistsAsync(id);
    }
}