#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class UserSubscriptionsController : Controller
{
    private readonly ILogger<UserSubscriptionsController> _logger;
    private readonly IAppPublic _public;

    public UserSubscriptionsController(IAppPublic appPublic, ILogger<UserSubscriptionsController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/UserSubscriptions
    public async Task<IActionResult> Index()
    {
        return View(await _public.UserSubscription.IncludeGetAllByUserIdAsync(User.GetUserId()));
    }

    // GET: Authorized/UserSubscriptions/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userSubscription =
            await _public.UserSubscription.IncludeFirstOrDefaultAsync(id.Value);
        if (userSubscription == null) return NotFound();

        return View(userSubscription);
    }

    // GET: Authorized/UserSubscriptions/Create
    public async Task<IActionResult> Create()
    {
        var vm = new UserSubscriptionCreateVM
        {
            SubscriptionSelectList = new SelectList((await _public.Subscription.GetAllAsync())
                .Select(s => new {s.Id, s.Naming}), nameof(Subscription.Id),
                nameof(Subscription.Naming))
        };
        return View(vm);
    }

    // POST: Authorized/UserSubscriptions/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserSubscriptionCreateVM vm)
    {
        vm.UserSubscription.AppUserId = User.GetUserId();
        if (ModelState.IsValid)
        {
            _public.UserSubscription.Add(vm.UserSubscription);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.SubscriptionSelectList = new SelectList((await _public.Subscription.GetAllAsync())
            .Select(s => new {s.Id, s.Naming}), nameof(Subscription.Id),
            nameof(Subscription.Naming), vm.UserSubscription.SubscriptionId);
        return View(vm);
    }

    // GET: Authorized/UserSubscriptions/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userSubscription =
            await _public.UserSubscription.IncludeFirstOrDefaultAsync(id.Value);
        if (userSubscription == null) return NotFound();

        return View(userSubscription);
    }

    // POST: Authorized/UserSubscriptions/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.UserSubscription.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}