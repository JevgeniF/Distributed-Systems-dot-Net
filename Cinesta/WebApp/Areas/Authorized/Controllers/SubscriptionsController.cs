#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class SubscriptionsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<SubscriptionsController> _logger;

    public SubscriptionsController(AppDbContext context, ILogger<SubscriptionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/Subscriptions
    public async Task<IActionResult> Index()
    {
        return View(await _context.Subscriptions.ToListAsync());
    }

    // GET: Authorized/Subscriptions/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
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
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(subscription);
    }

    // GET: Authorized/Subscriptions/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
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
                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(subscription.Id))
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

        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
        if (subscription == null) return NotFound();

        return View(subscription);
    }

    // POST: Authorized/Subscriptions/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        _context.Subscriptions.Remove(subscription!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubscriptionExists(Guid id)
    {
        return _context.Subscriptions.Any(e => e.Id == id);
    }

}