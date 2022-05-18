#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class UserSubscriptionsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserSubscriptionsController> _logger;

    public UserSubscriptionsController(AppDbContext context, ILogger<UserSubscriptionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/UserSubscriptions
    public async Task<IActionResult> Index()
    {
        return View(await _context.UserSubscriptions
            .Include(u => u.Subscription)
            .Include(u => u.AppUser)
            .Where(u => u.AppUserId == User.GetUserId()).ToListAsync());
    }

    // GET: Authorized/UserSubscriptions/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userSubscription =
            await _context.UserSubscriptions
                .Include(u => u.Subscription)
                .Include(u => u.AppUser).FirstOrDefaultAsync(u => u.Id == id);
        if (userSubscription == null) return NotFound();

        return View(userSubscription);
    }

    // GET: Authorized/UserSubscriptions/Create
    public async Task<IActionResult> Create()
    {
        var vm = new UserSubscriptionCreateVM
        {
            SubscriptionSelectList = new SelectList((await _context.Subscriptions.ToListAsync())
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
            _context.UserSubscriptions.Add(vm.UserSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.SubscriptionSelectList = new SelectList((await _context.Subscriptions.ToListAsync())
            .Select(s => new {s.Id, s.Naming}), nameof(Subscription.Id),
            nameof(Subscription.Naming), vm.UserSubscription.SubscriptionId);
        return View(vm);
    }

    // GET: Authorized/UserSubscriptions/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userSubscription =
            await _context.UserSubscriptions
                .Include(u => u.Subscription)
                .Include(u => u.AppUser).FirstOrDefaultAsync(u => u.Id == id);
        if (userSubscription == null) return NotFound();

        return View(userSubscription);
    }

    // POST: Authorized/UserSubscriptions/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userSubscription = await _context.UserSubscriptions.FindAsync(id);
        _context.UserSubscriptions.Remove(userSubscription!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserSubscriptionExists(Guid id)
    {
        return _context.UserSubscriptions.Any(u => u.Id == id);
    }
}