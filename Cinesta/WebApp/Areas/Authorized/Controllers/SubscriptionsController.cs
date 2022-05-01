#nullable disable
using App.Contracts.DAL;
using App.Domain.User;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class SubscriptionsController : Controller
{
    private readonly IAppUOW _uow;

    public SubscriptionsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/Subscriptions
    public async Task<IActionResult> Index()
    {
        return View(await _uow.Subscription.GetAllByUserIdAsync(User.GetUserId()));
    }

    // GET: Admin/Subscriptions/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _uow.Subscription.FirstOrDefaultAsync(id.Value);
        if (subscription == null) return NotFound();

        return View(subscription);
    }

    // GET: Admin/Subscriptions/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Subscriptions/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Subscription subscription)
    {
        if (ModelState.IsValid)
        {
            subscription.AppUserId = User.GetUserId();
            subscription.Id = Guid.NewGuid();
            _uow.Subscription.Add(subscription);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(subscription);
    }

    // GET: Subscriptions/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _uow.Subscription.FirstOrDefaultAsync(id.Value);
        if (subscription == null) return NotFound();
        return View(subscription);
    }

    // POST: Subscriptions/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Subscription subscription)
    {
        if (id != subscription.Id) return NotFound();

        subscription.AppUserId = User.GetUserId();

        if (ModelState.IsValid)
        {
            var subscriptionFromDb = await _uow.Subscription.FirstOrDefaultAsync(id);
            if (subscriptionFromDb == null) return NotFound();

            try
            {
                subscriptionFromDb.Naming.SetTranslation(subscription.Naming);
                subscription.Naming = subscriptionFromDb.Naming;

                subscriptionFromDb.Description.SetTranslation(subscription.Description);
                subscription.Description = subscriptionFromDb.Description;

                _uow.Subscription.Update(subscription);
                await _uow.SaveChangesAsync();
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


    // GET: Admin/Subscriptions/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _uow.Subscription.FirstOrDefaultAsync(id.Value);
        if (subscription == null) return NotFound();

        return View(subscription);
    }

    // POST: Admin/Subscriptions/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.Subscription.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> SubscriptionExists(Guid id)
    {
        return await _uow.Subscription.ExistsAsync(id);
    }
}