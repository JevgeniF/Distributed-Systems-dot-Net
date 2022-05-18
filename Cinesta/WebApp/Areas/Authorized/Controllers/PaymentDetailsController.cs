#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class PaymentDetailsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<PaymentDetailsController> _logger;

    public PaymentDetailsController(AppDbContext context, ILogger<PaymentDetailsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/PaymentDetails
    public async Task<IActionResult> Index()
    {
        return View(await _context.PaymentDetails.Include(p => p.AppUser)
            .Where(p => p.AppUserId == User.GetUserId()).ToListAsync());
    }

    // GET: Authorized/PaymentDetails/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _context.PaymentDetails
            .Include(p => p.AppUser)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (paymentDetails == null) return NotFound();

        return View(paymentDetails);
    }


    // GET: PaymentDetails/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PaymentDetails/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentDetails paymentDetails)
    {
        paymentDetails.AppUserId = User.GetUserId();
        if (ModelState.IsValid)
        {
            _context.PaymentDetails.Add(paymentDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(paymentDetails);
    }

    // GET: PaymentDetails/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _context.PaymentDetails.FindAsync(id);
        if (paymentDetails == null) return NotFound();
        return View(paymentDetails);
    }

    // POST: PaymentDetails/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, PaymentDetails paymentDetails)
    {
        if (id != paymentDetails.Id) return NotFound();

        paymentDetails.AppUserId = User.GetUserId();

        if (ModelState.IsValid)
        {
            try
            {
                _context.PaymentDetails.Update(paymentDetails);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentDetailsExists(paymentDetails.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(paymentDetails);
    }


    // GET: Authorized/PaymentDetails/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _context.PaymentDetails.Include(p => p.AppUser)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (paymentDetails == null) return NotFound();

        return View(paymentDetails);
    }

    // POST: Authorized/PaymentDetails/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var paymentDetails = await _context.PaymentDetails.FindAsync(id);
        _context.PaymentDetails.Remove(paymentDetails!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PaymentDetailsExists(Guid id)
    {
        return _context.PaymentDetails.Any(e => e.Id == id);
    }
}