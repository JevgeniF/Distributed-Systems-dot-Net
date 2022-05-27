#nullable disable
#pragma warning disable CS1591
using App.BLL.DTO;
using App.Contracts.BLL;
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
    private readonly IAppBll _bll;

    public UserSubscriptionsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Authorized/UserSubscriptions
    public async Task<IActionResult> Index()
    {
        return View(await _bll.UserSubscription.IncludeGetAllByUserIdAsync(User.GetUserId()));
    }

    // GET: Authorized/UserSubscriptions/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userSubscription =
            await _bll.UserSubscription.IncludeFirstOrDefaultAsync(id.Value);
        if (userSubscription == null) return NotFound();

        return View(userSubscription);
    }

    // GET: Authorized/UserSubscriptions/Create
    public async Task<IActionResult> Create()
    {
        var vm = new UserSubscriptionCreateVm
        {
            SubscriptionSelectList = new SelectList((await _bll.Subscription.GetAllAsync())
                .Select(s => new { s.Id, s.Naming }), nameof(Subscription.Id),
                nameof(Subscription.Naming))
        };
        return View(vm);
    }

    // POST: Authorized/UserSubscriptions/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserSubscriptionCreateVm vm)
    {
        if (ModelState.IsValid)
        {
            vm.UserSubscription.AppUserId = User.GetUserId();
            _bll.UserSubscription.Add(vm.UserSubscription);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.SubscriptionSelectList = new SelectList((await _bll.Subscription.GetAllAsync())
            .Select(s => new { s.Id, s.Naming }), nameof(Subscription.Id),
            nameof(Subscription.Naming), vm.UserSubscription.SubscriptionId);
        return View(vm);
    }

    // GET: Authorized/UserSubscriptions/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userSubscription =
            await _bll.UserSubscription.IncludeFirstOrDefaultAsync(id.Value);
        if (userSubscription == null) return NotFound();

        return View(userSubscription);
    }

    // POST: Authorized/UserSubscriptions/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.UserSubscription.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}